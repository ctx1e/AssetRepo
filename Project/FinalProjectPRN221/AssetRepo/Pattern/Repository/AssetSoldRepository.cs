// AssetSoldRepository.cs
using AssetRepo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetRepo.Pattern.Repository
{
    public class AssetSoldRepository : IAssetSoldRepository
    {
        private readonly AssetRepoContext _context;

        public AssetSoldRepository(AssetRepoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetSold>> GetAllSoldAssetsAsync(int pageNumber, int pageSize)
        {
            return await _context.AssetSolds
                .OrderByDescending(p => p.AssetSoldId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.Asset)
                .ToListAsync();
        }

        public async Task<AssetSold> GetSoldAssetByIdAsync(int id)
        {
            return await _context.AssetSolds
    .Include(p => p.Asset)
    .SingleOrDefaultAsync(p => p.AssetSoldId == id);
        }

        public async Task AddSoldAssetAsync(AssetSold assetSold)
        {
            await _context.AssetSolds.AddAsync(assetSold);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSoldAssetAsync(AssetSold assetSold)
        {
            _context.AssetSolds.Update(assetSold);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSoldAssetAsync(int id)
        {
            var assetSold = await _context.AssetSolds.FindAsync(id);
            if (assetSold != null)
            {
                _context.AssetSolds.Remove(assetSold);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetSoldAssetCountAsync()
        {
            return await _context.AssetSolds.CountAsync();
        }
    }
}
