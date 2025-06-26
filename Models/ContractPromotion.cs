using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_MEGA_PROJECT.Models
{
    public class ContractPromotion
    {


        public int ContractId { get; set; }
        public Contract Contract { get; set; }

     
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }

    }

}
