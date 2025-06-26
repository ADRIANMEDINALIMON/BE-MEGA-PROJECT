using System.ComponentModel.DataAnnotations;

namespace BE_MEGA_PROJECT.Models
{
    public class Branch
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        // Navegación opcional
        public ICollection<Subscriber> Subscribers { get; set; }

    }
}
