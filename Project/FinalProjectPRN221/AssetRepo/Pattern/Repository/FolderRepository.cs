using AssetRepo.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AssetRepo.Pattern.Repository
{
    public class FolderRepository : IFolderRepository
    {

        private readonly AssetRepoContext _context;
        public FolderRepository(AssetRepoContext context)
        {
            _context = context;
        }
        public async Task AddNewFolderAsync(Folder folder)
        {
            await _context.Folders.AddAsync(folder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFolderAsync(long folderId)
        {
            await DeleteFolderAndFile(folderId);
        }

        public async Task<IEnumerable<Folder>> GetAllFolderAsync()
        {
            return await _context.Folders.ToListAsync();
        }

        public async Task<Folder> GetFolderByIdAsync(long folderId)
        {
            return await _context.Folders.FindAsync(folderId);
        }

        public async Task<List<Folder>> GetFolderPath(long? folderId)
        {
            List<Folder> path = new List<Folder>();

            var currentFolder = _context.Folders.Find(folderId);
            while (currentFolder != null && currentFolder.ParentFolderId.HasValue)
            {
                path.Insert(0, currentFolder);
                currentFolder = _context.Folders.Find((long)currentFolder.ParentFolderId.Value);
            }

            if (currentFolder != null)
            {
                path.Insert(0, currentFolder); // Add the root folder if it exists
            }

            return path;
        }

        public async Task<IEnumerable<Folder>> GetRootSubFolderAsync(int assetId)
        {
            return await _context.Folders.Where(x => x.AssetId == assetId && x.ParentFolderId == null).ToListAsync();
        }

        public async Task<IEnumerable<Folder>> GetSubFolderAsync(int assetId, long? parentFolderId)
        {
            return await _context.Folders.Where(x => x.AssetId == assetId && x.ParentFolderId == parentFolderId).ToListAsync(); 
        }

        public async Task UpdateFolderAsync(Folder folder)
        {
            _context.Folders.Update(folder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFolderAndFile(long folderId)
        {
            var folder = await GetFolderByIdAsync(folderId);
            if (folder == null) return;

          
            var assetById = await _context.Assets.FindAsync(folder.AssetId);
            string folderName = (folder.FolderId == 0 || folder.FolderId == null) ? "root" : folder.FolderId.ToString();
            string folderPath = Path.Combine("wwwroot", "upload", assetById.AssetName, folderName);
            
            // Đệ quy để xóa các thư mục con
            var subFolders = await _context.Folders.Where(f => f.AssetId == folder.AssetId && f.ParentFolderId == folderId).ToListAsync();
            foreach (var subFolder in subFolders)
            {
               await DeleteFolderAndFile(subFolder.FolderId);
            }

            // Xóa tất cả các tệp trong thư mục hiện tại
            var files = await _context.Files.Where(f => f.FolderId == folderId).ToListAsync();

            // Delete in wwwroot
            foreach (var file in files)
            {
                // Define the file path
                string filePath = Path.Combine(folderPath, file.OriginalFileName);

                // Check if the file exists and delete it
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Files.RemoveRange(files);

            // Delete the folder directory from the file system if it exists
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true); // true ensures recursive deletion
            }

            _context.Folders.Remove(folder);
            await _context.SaveChangesAsync();

        }
    }
}
