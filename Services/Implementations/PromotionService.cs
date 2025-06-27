using BE_MEGA_PROJECT.Data;
using BE_MEGA_PROJECT.Enums;
using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using BE_MEGA_PROJECT.Services.Interfaces;

namespace BE_MEGA_PROJECT.Services.Implementations
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _repo;
        private readonly ApplicationDbContext _context;
        public PromotionService(IPromotionRepository repo, ApplicationDbContext context) {
            _repo = repo;
            _context = context;
        }
    
       public Task<IEnumerable<Promotion>> GetAll() => _repo.GetAllPromotions();

        public Task<Promotion?> GetById(int id) => _repo.GetPromotionById(id);

        public async Task<Promotion> Create(Promotion promotion)  {
            ValidateDates(promotion);
            await ValidateTargetExists(promotion);
            if (promotion.ServiceId.HasValue) {
                await ValidateServiceExists(promotion);
            }
            return await  _repo.CreatePromotion(promotion);
        }

        ///validaciones

        private void ValidateDates(Promotion promotion) {
            if (promotion.StartDate > promotion.EndDate)
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");
        }
        private async Task ValidateTargetExists(Promotion promotion) {
            bool exist = promotion.TargetType switch
            {
                TargetType.CITY => _context.Cities.Any(c => c.Id == promotion.TargetId),
                TargetType.NEIGHBORHOOD => _context.Neighborhoods.Any(n => n.Id == promotion.TargetId),
                TargetType.BRANCH => _context.Branches.Any(b => b.Id == promotion.TargetId),
                _ => false,
            };
            if (!exist) { 
                throw new ArgumentException($"El ID {promotion.ServiceId} no existe en la tabla {promotion.TargetType}");
        }

        }
        private async Task ValidateServiceExists(Promotion promotion)
        {
            if (promotion.ServiceId.HasValue)
            {
                bool serviceExist =  _context.Services.Any(s => s.Id == promotion.ServiceId.Value);
                if (!serviceExist)
                {
                    throw new ArgumentException($"El servicio con ID {promotion.ServiceId.Value} no existe.");
                }
            }

        }

        public async Task<bool> Update(Promotion promotion) {
            ValidateDates(promotion);
            await ValidateTargetExists(promotion);
            if (promotion.ServiceId.HasValue)
            {
                await ValidateServiceExists(promotion);
            }
            return await _repo.UpdatePromotion(promotion); 
        }
        public Task<bool> Delete(int id) =>
            _repo.DeletePromotion(id);
    } 

}
