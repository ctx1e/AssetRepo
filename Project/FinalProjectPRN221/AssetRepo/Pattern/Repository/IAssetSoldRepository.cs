using AssetRepo.Models;

namespace AssetRepo.Pattern.Repository
{
    public interface IAssetSoldRepository
    {
        Task<IEnumerable<AssetSold>> GetAllSoldAssetsAsync(int pageNumber, int pageSize);
        Task<AssetSold> GetSoldAssetByIdAsync(int id);
        Task AddSoldAssetAsync(AssetSold assetSold);
        Task UpdateSoldAssetAsync(AssetSold assetSold);
        Task DeleteSoldAssetAsync(int id);
        Task<int> GetSoldAssetCountAsync();
    }
}
