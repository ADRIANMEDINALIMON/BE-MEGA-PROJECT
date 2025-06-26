using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace BE_MEGA_PROJECT.Models
{
    public class Package
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<PackageService> PackageServices { get; set; }
        public ICollection<Contract> Contracts { get; set; }

    }
}
