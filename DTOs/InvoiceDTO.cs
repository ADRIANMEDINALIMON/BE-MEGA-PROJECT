namespace BE_MEGA_PROJECT.DTOs
{
    public class InvoiceDTO
    {

        public int Id { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
