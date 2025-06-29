using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace BE_MEGA_PROJECT.Models
{
    public class Subscriber
    {


        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public City City { get; set; }

        [ForeignKey("Neighborhood")]
        public int NeighborhoodId { get; set; }
        public Neighborhood Neighborhood { get; set; }

        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        [Required]
        [MaxLength(20)]
        public string AccountType { get; set; } // 'RESIDENTIAL' o 'BUSINESS'
        public ICollection<Invoice> Invoices { get; set; }

  

    }
}
