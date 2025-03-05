
using AssetRepo.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetRepo.Pattern.Repository
{
    public class FileRepository : IFIleRepository
    {

        private readonly AssetRepoContext _context;

        public FileRepository(AssetRepoContext context)
        {
            _context = context;
        }
        public async Task AddFileAsync(Models.File file)
        {
            await _context.AddAsync(file);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFileAsync(int fileId)
        {
            Models.File file = await GetFileById(fileId);
            if (file == null) return;

            // Define the file path based on the asset and folder structure

            string folderName = (file.FolderId == 0 || file.FolderId == null) ? "root" : file.FolderId.ToString();
            var assetById = await _context.Assets.FindAsync(file.AssetId);
            string folderPath = Path.Combine("wwwroot", "upload", assetById.AssetName, folderName);
            string filePath = Path.Combine(folderPath, file.OriginalFileName);

            // Check if the file exists and delete it
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Models.File>> GetAllFileAsync()
        {
            return await _context.Files.ToListAsync();
        }

        public async Task<IEnumerable<Models.File>> GetAllFileByFolderId(int assetId, long? folderId)
        {
            return await _context.Files.Where(x => x.AssetId == assetId && x.FolderId == folderId).ToListAsync();
        }

        public async Task<IEnumerable<Models.File>> GetAllRootFile(int assetId)
        {
            return await _context.Files.Where(x => x.AssetId == assetId && x.FolderId == null).ToListAsync();
        }

        public async Task<Models.File> GetFileById(int fileId)
        {
            return await _context.Files.FindAsync(fileId);
        }

        public async Task UpdateFileAsync(Models.File file)
        {
            _context.Files.Update(file);
            await _context.SaveChangesAsync();
        }
    }
}
