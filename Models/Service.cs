using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        public ICollection<PackageService> PackageServices { get; set; }
        [JsonIgnore]
        public ICollection<Promotion> Promotions { get; set; }

    }
}
