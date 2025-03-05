using AssetRepo.Models;
using AssetRepo.Pattern.Repository;
using Microsoft.EntityFrameworkCore;

namespace AssetRepo.Pattern.Service
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;

        public AssetService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<IEnumerable<Asset>> GetAllAssetsAsync(int pageNumber, int pageSize)
        {
            return await _assetRepository.GetAllAssetsAsync(pageNumber, pageSize);
        }

        public async Task<Asset> GetAssetByIdAsync(int id)
        {
            return await _assetRepository.GetAssetByIdAsync(id);
        }

        public async Task AddAssetAsync(Asset asset)
        {
            await _assetRepository.AddAssetAsync(asset);
        }

        public async Task UpdateAssetAsync(Asset asset)
        {
            await _assetRepository.UpdateAssetAsync(asset);
        }

        public async Task DeleteAssetAsync(int id)
        {
            await _assetRepository.DeleteAssetAsync(id);
        }
        public async Task<int> GetAssetCountAsync()
        {
            return await _assetRepository.GetAssetCountAsync();
        }
        
        public async Task<IEnumerable<Asset>> GetAvailableAssetsForProjectAsync()
        {
            return await _assetRepository.GetAvailableAssetsForProjectAsync();
        }
        public async Task<IEnumerable<Asset>> GetAvailableAssetsForSaleAsync(int? currentAssetId = null)
        {
            return await _assetRepository.GetAvailableAssetsForSaleAsync(currentAssetId);
        }

        public async Task<bool> IsNameAssetExistAsync(Asset asset)
        {
            return await _assetRepository.IsNameAssetExistAsync(asset);
        }

        public async Task<(List<Asset> Assets, int TotalCount)> SearchAssetsByNameAsync(string searchTerm, int pageNumber, int pageSize)

        {
            return await _assetRepository.SearchAssetsByNameAsync(searchTerm, pageNumber, pageSize);
        }
    }
}
