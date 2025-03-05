using AssetRepo.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetRepo.Pattern.Repository
{
    public class AssetRepository : IAssetRepository
    {
        private readonly AssetRepoContext _context;

        public AssetRepository(AssetRepoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Asset>> GetAllAssetsAsync(int pageNumber, int pageSize)
        {
            return await _context.Assets
        .OrderByDescending(c => c.CreatedDate)
        
        .Include(a => a.Projects)
        .Include(a => a.AssetSold)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
        }

        public async Task<Asset> GetAssetByIdAsync(int id)
        {
            return await _context.Assets.FindAsync(id);
        }

        public async Task AddAssetAsync(Asset asset)
        {
            await _context.Assets.AddAsync(asset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAssetAsync(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssetAsync(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset != null)
            {
                string assetFolderPath = Path.Combine("wwwroot", "upload", asset.AssetName);
                // 1. Xóa trong AssetSold
                var assetSold = await _context.AssetSolds.FirstOrDefaultAsync(a => a.AssetId == id);
                if (assetSold != null)
                {
                    _context.AssetSolds.Remove(assetSold);
                }

                // 2. Xóa tất cả các File liên quan đến Asset này (bao gồm File ở cấp độ gốc)
                var files = _context.Files.Where(f => f.AssetId == id).ToList();
                foreach (var file in files)
                {

                    string folderName = (file.FolderId == 0 || file.FolderId == null) ? "root" : file.FolderId.ToString();
                    string folderPath = Path.Combine(assetFolderPath, folderName);
                    string filePath = Path.Combine(folderPath, file.OriginalFileName);


                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _context.Files.RemoveRange(files);

                // 3. Xóa tất cả các Folder liên quan đến Asset này
                var folders = _context.Folders.Where(f => f.AssetId == id).ToList();
                foreach (var folder in folders)
                {
                    string folderName = (folder.FolderId == 0 || folder.FolderId == null) ? "root" : folder.FolderId.ToString();
                    string folderPath = Path.Combine(assetFolderPath, folderName);
                    if (Directory.Exists(folderPath))
                    {
                        Directory.Delete(folderPath, true);
                    }
                }
                _context.Folders.RemoveRange(folders);

                // 4. Cập nhật AssetID về NULL trong bảng Projects
                var projects = _context.Projects.Where(p => p.AssetId == id).ToList();
                foreach (var project in projects)
                {
                    project.AssetId = null;
                }
                _context.Projects.UpdateRange(projects);

                // 5. Cuối cùng, xóa Asset
                _context.Assets.Remove(asset);

                if (Directory.Exists(assetFolderPath))
                {
                    Directory.Delete(assetFolderPath, true);
                }

                // Lưu tất cả các thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> GetAssetCountAsync()
        {

            return await _context.Assets.CountAsync();

        }
        public async Task<IEnumerable<Asset>> GetAvailableAssetsForProjectAsync()
        {
            // Lấy tất cả AssetId trong AssetSold
            var soldAssetIds = await _context.AssetSolds
                .Select(s => s.AssetId)
                .ToListAsync();

            // Lấy danh sách tài sản không nằm trong AssetSold
            var availableAssets = await _context.Assets
                .Where(asset => !soldAssetIds.Contains(asset.AssetId))
                .ToListAsync();

            return availableAssets;
        }
        public async Task<bool> IsNameAssetExistAsync(Asset asset)
        {
            if (string.IsNullOrWhiteSpace(asset.AssetName))
            {
                return false; // Return false if assetName is null or empty
            }

            return await _context.Assets
        .AnyAsync(a => a.AssetName.ToLower() == asset.AssetName.ToLower() && a.AssetId != asset.AssetId);
        }

        public async Task<(List<Asset> Assets, int TotalCount)> SearchAssetsByNameAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var totalCount = await _context.Assets
       .Where(asset => asset.AssetName.ToLower().Contains(searchTerm.ToLower()))
       .CountAsync();

            // Lấy kết quả phân trang
            var assets = await _context.Assets
                .Where(asset => asset.AssetName.ToLower().Contains(searchTerm.ToLower()))
                .OrderByDescending(c => c.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (assets, totalCount);
        }
        public async Task<IEnumerable<Asset>> GetAvailableAssetsForSaleAsync()
        {
            // Lấy tất cả AssetId trong bảng Project
            var projectAssetIds = await _context.Projects
                .Where(p => p.AssetId.HasValue)
                .Select(p => p.AssetId.Value)
                .ToListAsync();

            // Lấy tất cả AssetId trong bảng AssetSold
            var soldAssetIds = await _context.AssetSolds
                .Where(s => s.AssetId.HasValue)
                .Select(s => s.AssetId.Value)
                .ToListAsync();

            // Lấy danh sách Asset không thuộc bất kỳ Project hoặc AssetSold nào
            var availableAssets = await _context.Assets
                .Where(asset => !projectAssetIds.Contains(asset.AssetId) && !soldAssetIds.Contains(asset.AssetId))
                .ToListAsync();

            return availableAssets;
        }

        public async Task<IEnumerable<Asset>> GetAvailableAssetsForSaleAsync(int? currentAssetId = null)
        {
            // Lấy tất cả AssetId trong bảng Project
            var projectAssetIds = await _context.Projects
                .Where(p => p.AssetId.HasValue)
                .Select(p => p.AssetId.Value)
                .ToListAsync();

            // Lấy tất cả AssetId trong bảng AssetSold
            var soldAssetIds = await _context.AssetSolds
                .Where(s => s.AssetId.HasValue)
                .Select(s => s.AssetId.Value)
                .ToListAsync();

            // Lấy danh sách Asset không thuộc bất kỳ Project hoặc AssetSold nào
            var availableAssets = await _context.Assets
                .Where(asset => (!projectAssetIds.Contains(asset.AssetId) && !soldAssetIds.Contains(asset.AssetId))
                                || asset.AssetId == currentAssetId)
                .ToListAsync();

            return availableAssets;
        }
    }
}
