using System.ComponentModel.DataAnnotations;

namespace BE_MEGA_PROJECT.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public decimal MonthlyPrice { get; set; }
        public decimal SetupPrice { get; set; }

        // Navegación inversa
        public ICollection<PackageService> PackageServices { get; set; }
        public ICollection<Promotion> Promotions { get; set; }

    }
}
