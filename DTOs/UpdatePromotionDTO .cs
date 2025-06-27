using System.ComponentModel.DataAnnotations;

namespace BE_MEGA_PROJECT.DTOs
{
    public class UpdatePromotionDTO: CreatePromotionDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
