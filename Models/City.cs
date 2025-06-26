using System.ComponentModel.DataAnnotations;

namespace BE_MEGA_PROJECT.Models
{
    public class City
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Navegación opcional
        public ICollection<Neighborhood> Neighborhoods { get; set; }
        public ICollection<Subscriber> Subscribers { get; set; }



    }
}
