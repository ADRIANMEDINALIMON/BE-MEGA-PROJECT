using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_MEGA_PROJECT.Models
{
    public class InvoicePromotion
    {

        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        [ForeignKey("Promotion")]
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        public decimal DiscountAmount { get; set; }

    }
}
