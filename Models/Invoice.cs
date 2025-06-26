using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_MEGA_PROJECT.Models
{
    public class Invoice
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("Subscriber")]
        public int SubscriberId { get; set; }
        public Subscriber Subscriber { get; set; }

        [ForeignKey("Contract")]
        public int ContractId { get; set; }
        public Contract Contract { get; set; }

        [Required]
        public DateTime PeriodStart { get; set; }

        [Required]
        public DateTime PeriodEnd { get; set; }

        public decimal BaseAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        public ICollection<InvoicePromotion> InvoicePromotions { get; set; }
    }
}
