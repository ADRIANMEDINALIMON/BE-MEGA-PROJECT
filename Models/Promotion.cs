using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BE_MEGA_PROJECT.Enums;

namespace BE_MEGA_PROJECT.Models
{
    public class Promotion
    {

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [MaxLength(10)] // 'PERCENT' o 'FIXED'
        public DiscountType DiscountType { get; set; }

        public decimal DiscountAmount { get; set; }

        [Required]
        [MaxLength(20)] // 'CITY', 'PACKAGE', etc.
        public TargetType TargetType { get; set; }

        public int TargetId { get; set; }

        [Required]
        [MaxLength(20)] // 'MONTHLY' o 'SETUP'
        public AppliesTo AppliesTo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Active { get; set; } = true;

        [ForeignKey("Service")]
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }

        public ICollection<ContractPromotion> ContractPromotions { get; set; }
        public ICollection<InvoicePromotion> InvoicePromotions { get; set; }

    }
}
