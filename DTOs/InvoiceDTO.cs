namespace BE_MEGA_PROJECT.DTOs
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal MonthlyBaseAmount { get; set; }
        public decimal MonthlyDiscountedAmount { get; set; }
        public decimal SetupBaseAmount { get; set; }
        public decimal SetupDiscountedAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal MonthlyFinalAmount => MonthlyBaseAmount - MonthlyDiscountedAmount;
        public decimal SetupFinalAmount => SetupBaseAmount - SetupDiscountedAmount;
        public decimal TotalBaseAmount => MonthlyBaseAmount + SetupBaseAmount;
        public decimal TotalDiscountAmount => MonthlyDiscountedAmount + SetupDiscountedAmount;

    }
}
