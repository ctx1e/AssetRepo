
using AssetRepo.Pattern.Repository;

namespace AssetRepo.Pattern.Service
{
    public class FileService : IFileService
    {

        private readonly IFIleRepository _repository;

        public FileService(IFIleRepository repository)
        {
            _repository = repository;
        }
        public async Task AddFileAsync(Models.File file)
        {
            await _repository.AddFileAsync(file);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            await _repository.DeleteFileAsync(fileId);
        }

        public async Task<IEnumerable<Models.File>> GetAllFileAsync()
        {
            return await _repository.GetAllFileAsync();
        }

        public async Task<IEnumerable<Models.File>> GetAllFileByFolderId(int assetId, long? folderId)
        {
            return await _repository.GetAllFileByFolderId(assetId, folderId);
        }

        public async Task<IEnumerable<Models.File>> GetAllRootFile(int assetId)
        {
            return await _repository.GetAllRootFile(assetId);
        }

        public async Task<Models.File> GetFileById(int fileId)
        {
            return await _repository.GetFileById(fileId);
        }

        public async Task UpdateFileAsync(Models.File file)
        {
            await _repository.UpdateFileAsync(file);
        }
    }
}
