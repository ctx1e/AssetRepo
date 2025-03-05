using AssetRepo.Models;

namespace AssetRepo.Pattern.Service
{
    public interface IAssetService
    {
        Task<IEnumerable<Asset>> GetAllAssetsAsync(int pageNumber, int pageSize);
        Task<Asset> GetAssetByIdAsync(int id);
        Task AddAssetAsync(Asset asset);
        Task UpdateAssetAsync(Asset asset);
        Task DeleteAssetAsync(int id);
        Task<int> GetAssetCountAsync();

        Task<bool> IsNameAssetExistAsync(Asset asset);
        Task<(List<Asset> Assets, int TotalCount)> SearchAssetsByNameAsync(string searchTerm, int pageNumber, int pageSize);

        Task<IEnumerable<Asset>> GetAvailableAssetsForProjectAsync();
        Task<IEnumerable<Asset>> GetAvailableAssetsForSaleAsync(int? currentAssetId = null);
    }
}
