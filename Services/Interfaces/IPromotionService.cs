using BE_MEGA_PROJECT.Models;

namespace BE_MEGA_PROJECT.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<IEnumerable<Promotion>> GetAll();
        Task<Promotion?> GetById(int id);
        Task<Promotion> Create(Promotion promotion);
        Task<bool> Update(Promotion promotion);
        Task<bool> Delete(int id);
    }
}
