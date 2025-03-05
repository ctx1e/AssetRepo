// AssetSoldService.cs

using AssetRepo.Models;
using AssetRepo.Pattern.Repository;

namespace AssetRepo.Pattern.Service
{
    public class AssetSoldService : IAssetSoldService
    {
        private readonly IAssetSoldRepository _assetSoldRepository;

        public AssetSoldService(IAssetSoldRepository assetSoldRepository)
        {
            _assetSoldRepository = assetSoldRepository;
        }

        public async Task<IEnumerable<AssetSold>> GetAllSoldAssetsAsync(int pageNumber, int pageSize)
        {
            return await _assetSoldRepository.GetAllSoldAssetsAsync(pageNumber, pageSize);
        }

        public async Task<AssetSold> GetSoldAssetByIdAsync(int id)
        {
            return await _assetSoldRepository.GetSoldAssetByIdAsync(id);
        }

        public async Task AddSoldAssetAsync(AssetSold assetSold)
        {
            await _assetSoldRepository.AddSoldAssetAsync(assetSold);
        }

        public async Task UpdateSoldAssetAsync(AssetSold assetSold)
        {
            await _assetSoldRepository.UpdateSoldAssetAsync(assetSold);
        }

        public async Task DeleteSoldAssetAsync(int id)
        {
            await _assetSoldRepository.DeleteSoldAssetAsync(id);
        }

        public async Task<int> GetSoldAssetCountAsync()
        {
            return await _assetSoldRepository.GetSoldAssetCountAsync();
        }
    }
}
