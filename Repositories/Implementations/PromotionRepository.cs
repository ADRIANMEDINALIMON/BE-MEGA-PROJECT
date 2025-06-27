using BE_MEGA_PROJECT.Data;
using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_MEGA_PROJECT.Repositories.Implementations
{
    public class PromotionRepository: IPromotionRepository
    {
        private readonly ApplicationDbContext _context;

        public PromotionRepository(ApplicationDbContext context) { 
            _context = context;
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotions()
        {
            return await _context.Promotions
                .Include(p => p.Service)
                .ToListAsync();
        }

        public async Task<Promotion?> GetPromotionById(int id)
        {
            return await _context.Promotions
                .Include(p => p.Service)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Promotion> CreatePromotion(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
            return promotion;
        }

        public async Task<bool> UpdatePromotion(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePromotion(int id)
        {
            var promo = await _context.Promotions.FindAsync(id);
            if (promo == null) return false;
            _context.Promotions.Remove(promo);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
