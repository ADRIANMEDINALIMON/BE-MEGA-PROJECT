using BE_MEGA_PROJECT.Data;
using BE_MEGA_PROJECT.Enums;
using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.DTOs;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_MEGA_PROJECT.Repositories.Implementations
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceDTO> GenerateInvoice(int subscriberId, DateTime periodStart, DateTime periodEnd)
        {
            var contract = await GetActiveContractFor(subscriberId, periodStart);
            if (contract == null)
                throw new Exception("El suscriptor no tiene contrato activo en este período.");

            var services = GetContractServices(contract).ToList();

            // Base mensual
            decimal monthlyBaseAmount = services.Sum(s => s.MonthlyPrice);

            // Detectar si es la primera factura
            bool isFirstInvoice = contract.StartDate.Month == periodStart.Month &&
                                  contract.StartDate.Year == periodStart.Year;

            // Base de instalación solo si es la primera factura
            decimal setupBaseAmount = isFirstInvoice ? services.Sum(s => s.SetupPrice) : 0;

            // Obtener promociones aplicables
            var promotions = await GetApplicablePromotions(contract, periodStart, periodEnd);

            // Calcular descuentos
            var (monthlyDiscount, setupDiscount, invoicePromotions) =
                CalculateDetailedDiscounts(services, promotions, monthlyBaseAmount, setupBaseAmount);

            var invoice = new Invoice
            {
                SubscriberId = subscriberId,
                ContractId = contract.Id,
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                BaseAmount = monthlyBaseAmount + setupBaseAmount,
                TotalDiscount = monthlyDiscount + setupDiscount,
                TotalAmount = (monthlyBaseAmount + setupBaseAmount) - (monthlyDiscount + setupDiscount),
                GeneratedAt = DateTime.Now
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Guardar relación con promociones
            foreach (var ip in invoicePromotions)
            {
                ip.InvoiceId = invoice.Id;
                _context.InvoicePromotions.Add(ip);
            }
            await _context.SaveChangesAsync();

            return new InvoiceDTO
            {
                Id = invoice.Id,
                PeriodStart = invoice.PeriodStart,
                PeriodEnd = invoice.PeriodEnd,
                MonthlyBaseAmount = monthlyBaseAmount,
                MonthlyDiscountedAmount = monthlyDiscount,
                SetupBaseAmount = setupBaseAmount,
                SetupDiscountedAmount = setupDiscount,
                TotalAmount = invoice.TotalAmount
            };
        }

        private async Task<Contract?> GetActiveContractFor(int subscriberId, DateTime startDate)
        {
            return await _context.Contracts
                .Include(c => c.Package)
                    .ThenInclude(p => p.PackageServices)
                        .ThenInclude(ps => ps.Service)
                .Include(c => c.Subscriber)
                .FirstOrDefaultAsync(c => c.SubscriberId == subscriberId &&
                                          (c.EndDate == null || c.EndDate >= startDate));
        }

        private List<Service> GetContractServices(Contract contract)
        {
            return contract.Package.PackageServices.Select(ps => ps.Service).ToList();
        }

        private async Task<List<Promotion>> GetApplicablePromotions(Contract contract, DateTime start, DateTime end)
        {
            return await _context.Promotions
                .Where(p =>
                    p.Active &&
                    p.StartDate <= end &&
                    p.EndDate >= start &&
                    (
                        (p.TargetType == TargetType.CITY && p.TargetId == contract.Subscriber.CityId) ||
                        (p.TargetType == TargetType.NEIGHBORHOOD && p.TargetId == contract.Subscriber.NeighborhoodId) ||
                        (p.TargetType == TargetType.BRANCH && p.TargetId == contract.Subscriber.BranchId) ||
                        (p.TargetType == TargetType.PACKAGE && p.TargetId == contract.PackageId)
                    )
                ).ToListAsync();
        }

        private (decimal monthly, decimal setup, List<InvoicePromotion>) CalculateDetailedDiscounts(
            List<Service> services,
            List<Promotion> promotions,
            decimal monthlyBase,
            decimal setupBase)
        {
            decimal monthlyDiscount = 0;
            decimal setupDiscount = 0;
            var invoicePromos = new List<InvoicePromotion>();

            foreach (var promo in promotions)
            {
                decimal discount = 0;

                if (promo.AppliesTo == AppliesTo.MONTHLY)
                {
                    if (promo.ServiceId.HasValue)
                    {
                        var service = services.FirstOrDefault(s => s.Id == promo.ServiceId.Value);
                        if (service != null)
                        {
                            discount = promo.DiscountType switch
                            {
                                DiscountType.FIXED => promo.DiscountAmount,
                                DiscountType.PERCENT => service.MonthlyPrice * promo.DiscountAmount / 100,
                                _ => 0
                            };
                        }
                    }
                    else
                    {
                        discount = promo.DiscountType switch
                        {
                            DiscountType.FIXED => promo.DiscountAmount,
                            DiscountType.PERCENT => monthlyBase * promo.DiscountAmount / 100,
                            _ => 0
                        };
                    }
                    monthlyDiscount += discount;
                }
                else if (promo.AppliesTo == AppliesTo.SETUP)
                {
                    if (promo.ServiceId.HasValue)
                    {
                        var service = services.FirstOrDefault(s => s.Id == promo.ServiceId.Value);
                        if (service != null)
                        {
                            discount = promo.DiscountType switch
                            {
                                DiscountType.FIXED => promo.DiscountAmount,
                                DiscountType.PERCENT => service.SetupPrice * promo.DiscountAmount / 100,
                                _ => 0
                            };
                        }
                    }
                    else
                    {
                        discount = promo.DiscountType switch
                        {
                            DiscountType.FIXED => promo.DiscountAmount,
                            DiscountType.PERCENT => setupBase * promo.DiscountAmount / 100,
                            _ => 0
                        };
                    }
                    setupDiscount += discount;
                }

                if (discount > 0)
                {
                    invoicePromos.Add(new InvoicePromotion
                    {
                        PromotionId = promo.Id,
                        DiscountAmount = discount
                    });
                }
            }

            return (monthlyDiscount, setupDiscount, invoicePromos);
        }

        // Método original mantenido para compatibilidad si es necesario
        public async Task<Invoice> GenerateInvoiceOriginal(int subscriberId, DateTime periodStart, DateTime periodEnd)
        {
            var contract = await GetActiveContractFor(subscriberId, periodStart);
            if (contract == null)
                throw new Exception("El suscriptor no tiene contrato activo en este período.");

            var services = GetContractServices(contract);

            var baseAmount = services.Sum(s => s.MonthlyPrice);

            var applicablePromotions = await GetApplicablePromotions(contract, periodStart, periodEnd);

            var (totalDiscount, invoicePromotions) = CalculateTotalDiscount(services, applicablePromotions, baseAmount);

            var invoice = new Invoice
            {
                SubscriberId = subscriberId,
                ContractId = contract.Id,
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                BaseAmount = baseAmount,
                TotalDiscount = totalDiscount,
                TotalAmount = baseAmount - totalDiscount,
                GeneratedAt = DateTime.Now,
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        // Método original de cálculo de descuentos mantenido para compatibilidad
        private (decimal total, List<InvoicePromotion>) CalculateTotalDiscount(IEnumerable<Service> services, List<Promotion> promotions, decimal baseAmount)
        {
            decimal total = 0;
            var invoicePromos = new List<InvoicePromotion>();

            foreach (var promo in promotions)
            {
                decimal discount = 0;

                if (promo.ServiceId.HasValue)
                {
                    var service = services.FirstOrDefault(s => s.Id == promo.ServiceId.Value);
                    if (service != null)
                    {
                        discount = promo.DiscountType switch
                        {
                            DiscountType.FIXED => promo.DiscountAmount,
                            DiscountType.PERCENT => service.MonthlyPrice * promo.DiscountAmount / 100,
                            _ => 0
                        };
                    }
                }
                else
                {
                    discount = promo.DiscountType switch
                    {
                        DiscountType.FIXED => promo.DiscountAmount,
                        DiscountType.PERCENT => baseAmount * promo.DiscountAmount / 100,
                        _ => 0
                    };
                }

                total += discount;
                invoicePromos.Add(new InvoicePromotion
                {
                    PromotionId = promo.Id,
                    DiscountAmount = discount
                });
            }

            return (total, invoicePromos);
        }
    }
}