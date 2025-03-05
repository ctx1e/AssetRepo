using AssetRepo.Models;
using AssetRepo.Pattern.Repository;

namespace AssetRepo.Pattern.Service
{
    public class FolderService : IFolderService
    {

        private readonly IFolderRepository _repository;
        public FolderService(IFolderRepository repository)
        {
            _repository = repository;
        }
        public async Task AddNewFolderAsync(Folder folder)
        {
             await _repository.AddNewFolderAsync(folder);
        }

        public async Task DeleteFolderAsync(long folderId)
        {
            await _repository.DeleteFolderAsync(folderId);
        }

        public async Task<IEnumerable<Folder>> GetAllFolderAsync()
        {
            return await _repository.GetAllFolderAsync();
        }

        public async Task<Folder> GetFolderByIdAsync(long folderId)
        {
            return await _repository.GetFolderByIdAsync(folderId);
        }

        public async Task<List<Folder>> GetFolderPath(long? folderId)
        {
            return await _repository.GetFolderPath(folderId);
        }

        public async Task<IEnumerable<Folder>> GetRootSubFolderAsync(int assetId)
        {
            return await _repository.GetRootSubFolderAsync((int)assetId);
        }

        public async Task<IEnumerable<Folder>> GetSubFolderAsync(int assetId, long? parentFolderId)
        {
            return await _repository.GetSubFolderAsync((int)assetId, parentFolderId);
        }

        public async Task UpdateFolderAsync(Folder folder)
        {
            await _repository.UpdateFolderAsync(folder);
        }
    }
}
