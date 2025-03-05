namespace AssetRepo.Pattern.Repository
{
    public interface IFIleRepository
    {
        Task<IEnumerable<Models.File>> GetAllFileAsync();
        Task<Models.File> GetFileById(int fileId);

        Task AddFileAsync(Models.File file);
        Task UpdateFileAsync(Models.File file);
        Task DeleteFileAsync(int fileId);

        Task<IEnumerable<Models.File>> GetAllRootFile(int assetId);
        Task<IEnumerable<Models.File>> GetAllFileByFolderId(int assetId, long? folderId);
    }
}
