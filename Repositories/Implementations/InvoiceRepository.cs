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
        public async Task<InvoiceDTO> GenerateInvoice(int subscriberId, DateTime rangeStart, DateTime rangeEnd)
        {
            var contract = await GetActiveContractFor(subscriberId, rangeStart);
            if (contract == null)
                throw new Exception("El suscriptor no tiene contrato activo en este período.");

            var services = GetContractServices(contract).ToList();

            decimal monthlyBaseAmount = services.Sum(s => s.MonthlyPrice);
            decimal setupBaseAmount = 0;

            
            decimal totalMonthlyDiscount = 0;
            decimal totalSetupDiscount = 0;
            decimal totalAmount = 0;

            var currentStart = new DateTime(rangeStart.Year, rangeStart.Month, 1);
            var currentEnd = new DateTime(rangeEnd.Year, rangeEnd.Month, 1);

            bool setupAlreadyApplied = false;

            while (currentStart <= currentEnd)
            {
                DateTime periodStart = currentStart;
                DateTime periodEnd = currentStart.AddMonths(1).AddDays(-1);

                bool isFirstInvoice = contract.StartDate.Month == periodStart.Month &&
                                      contract.StartDate.Year == periodStart.Year;

                decimal currentSetupBase = (!setupAlreadyApplied && isFirstInvoice)
                    ? services.Sum(s => s.SetupPrice)
                    : 0;

                if (currentSetupBase > 0)
                    setupBaseAmount = currentSetupBase;

                var promotions = await GetApplicablePromotions(contract, periodStart, periodEnd);

                var (monthlyDiscount, setupDiscount) =
    CalculateDetailedDiscounts(services, promotions, monthlyBaseAmount, currentSetupBase, isFirstInvoice);

                totalMonthlyDiscount += monthlyDiscount;
                totalSetupDiscount += setupDiscount;

                decimal baseAmount = monthlyBaseAmount + currentSetupBase;
                decimal totalDisc = monthlyDiscount + setupDiscount;
                totalAmount += baseAmount - totalDisc;

                if (currentSetupBase > 0)
                    setupAlreadyApplied = true;

                currentStart = currentStart.AddMonths(1);
            }

            return new InvoiceDTO
            {
                PeriodStart = rangeStart,
                PeriodEnd = rangeEnd,
                MonthlyBaseAmount = monthlyBaseAmount,             
                MonthlyDiscountedAmount = totalMonthlyDiscount,    
                SetupBaseAmount = setupBaseAmount,                
                SetupDiscountedAmount = totalSetupDiscount,        
                TotalAmount = totalAmount                        
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
        private (decimal monthly, decimal setup) CalculateDetailedDiscounts(
     List<Service> services,
     List<Promotion> promotions,
     decimal monthlyBase,
     decimal setupBase,
     bool isFirstInvoice)
        {
            decimal monthlyDiscount = 0;
            decimal setupDiscount = 0;

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

                    if (discount > 0)
                        monthlyDiscount += discount;
                }

                else if (promo.AppliesTo == AppliesTo.SETUP)
                {
                    if (!isFirstInvoice || setupBase == 0)
                        continue;

                    if (promo.ServiceId.HasValue)
                    {
                        var service = services.FirstOrDefault(s => s.Id == promo.ServiceId.Value);
                        if (service != null && service.SetupPrice > 0)
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

                    if (discount > 0)
                        setupDiscount += discount;
                }
            }

            return (monthlyDiscount, setupDiscount);
        }


       public async Task<Invoice> GenerateInvoiceOriginal(int subscriberId, DateTime periodStart, DateTime periodEnd)
        {
            var contract = await GetActiveContractFor(subscriberId, periodStart);
            if (contract == null)
                throw new Exception("El suscriptor no tiene contrato activo en este período.");

            var services = GetContractServices(contract);

            var baseAmount = services.Sum(s => s.MonthlyPrice);

            var applicablePromotions = await GetApplicablePromotions(contract, periodStart, periodEnd);

            var totalDiscount = CalculateTotalDiscount(services, applicablePromotions, baseAmount);

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

        
        private decimal CalculateTotalDiscount(IEnumerable<Service> services, List<Promotion> promotions, decimal baseAmount)
        {
            decimal total = 0;

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
            }

            return total;
        }

    }
}