// Models/City.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace BE_MEGA_PROJECT.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Neighborhood> Neighborhoods { get; set; }

        [JsonIgnore]
        public ICollection<Subscriber> Subscribers { get; set; }
    }
}
