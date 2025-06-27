using BE_MEGA_PROJECT.Enums;
using System.ComponentModel.DataAnnotations;

namespace BE_MEGA_PROJECT.DTOs
{
    public class CreatePromotionDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal DiscountAmount { get; set; }

        [Required]
        public TargetType TargetType { get; set; }

        public int TargetId { get; set; }

        [Required]
        public AppliesTo AppliesTo { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool Active { get; set; } = true;

        public int? ServiceId { get; set; }
    }
}
