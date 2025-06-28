using BE_MEGA_PROJECT.Data;
using BE_MEGA_PROJECT.Enums;
using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_MEGA_PROJECT.Repositories.Implementations
{
    public class InvoiceRepository: IInvoiceRepository
    {

        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context) {
            _context = context;
        }
        
        public async Task<Invoice> GenerateInvoice(int subscriberId, DateTime periodStart, DateTime periodEnd)
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
                ///InvoicePromotions = invoicePromotions 
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }
        ///other functions
        ///
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

        private IEnumerable<Service> GetContractServices(Contract contract)
        {
            return contract.Package.PackageServices.Select(ps => ps.Service);
        }

        /////con fallos 

        private async Task<List<Promotion>> GetApplicablePromotions(Contract contract, DateTime start, DateTime end)
        {
            Console.WriteLine($"→ Verificando promociones para SubscriberId: {contract.SubscriberId}, NeighborhoodId: {contract.Subscriber.NeighborhoodId}, Expected: 3");
            Console.WriteLine($"→ BranchId: {contract.Subscriber.BranchId}, CityId: {contract.Subscriber.CityId}, PackageId: {contract.PackageId}");

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
