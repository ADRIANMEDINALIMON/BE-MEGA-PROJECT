using BE_MEGA_PROJECT.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_MEGA_PROJECT.DTOs
{
    public class UpdatePromotionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

   
        public DiscountType DiscountType { get; set; }

        public decimal DiscountAmount { get; set; }

       
        public TargetType TargetType { get; set; }

        public int TargetId { get; set; }


        public AppliesTo AppliesTo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Active { get; set; } = true;

        public int? ServiceId { get; set; }
       
    }
}
