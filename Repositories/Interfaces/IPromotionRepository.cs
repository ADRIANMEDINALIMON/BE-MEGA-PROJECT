using BE_MEGA_PROJECT.Models;

namespace BE_MEGA_PROJECT.Repositories.Interfaces
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<Promotion>> GetAllPromotions();
        Task<Promotion?> GetPromotionById(int id);
        Task<Promotion> CreatePromotion(Promotion promotion);
        Task<bool> UpdatePromotion(Promotion promotion);
        Task<bool> DeletePromotion(int id);
    }
}
