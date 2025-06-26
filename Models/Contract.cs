using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_MEGA_PROJECT.Models
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Subscriber")]
        public int SubscriberId { get; set; }
        public Subscriber Subscriber { get; set; }

        [ForeignKey("Package")]
        public int PackageId { get; set; }
        public Package Package { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ICollection<ContractPromotion> ContractPromotions { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }
}
