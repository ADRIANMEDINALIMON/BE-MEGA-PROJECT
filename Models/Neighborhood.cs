// Models/Neighborhood.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace BE_MEGA_PROJECT.Models
{
    public class Neighborhood
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }

        // Esta referencia al padre no se serializará
        [JsonIgnore]
        public City City { get; set; }

        // Igual con los subscribers si también vuelven al neighborhood
        [JsonIgnore]
        public ICollection<Subscriber> Subscribers { get; set; }
    }
}
