using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using BE_MEGA_PROJECT.Services.Interfaces;

namespace BE_MEGA_PROJECT.Services.Implementations
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _repo;

        public PromotionService(IPromotionRepository repo) {
            _repo = repo;
        }
    
       public Task<IEnumerable<Promotion>> GetAll() => _repo.GetAllPromotions();
        public Task<Promotion?> GetById(int id) => _repo.GetPromotionById(id);
        public Task<Promotion> Create(Promotion promotion) => _repo.CreatePromotion(promotion);
        public Task<bool> Update(Promotion promotion) => _repo.UpdatePromotion(promotion);
        public Task<bool> Delete(int id) => _repo.DeletePromotion(id);
    } 

}
