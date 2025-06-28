namespace BE_MEGA_PROJECT.DTOs
{
    public class InvoiceCalculationRequest
    {
        public int SubscriberId { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
