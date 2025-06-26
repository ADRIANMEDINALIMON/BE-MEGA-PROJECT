using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_MEGA_PROJECT.Models
{
    public class PackageService
    {

        public int PackageId { get; set; }
        public Package Package { get; set; }

       
        public int ServiceId { get; set; }
        public Service Service { get; set; }

    }
}
