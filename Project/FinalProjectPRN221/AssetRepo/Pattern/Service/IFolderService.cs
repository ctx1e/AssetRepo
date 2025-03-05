﻿using AssetRepo.Models;

namespace AssetRepo.Pattern.Service
{
    public interface IFolderService
    {
        Task<IEnumerable<Models.Folder>> GetAllFolderAsync();
        Task<Models.Folder> GetFolderByIdAsync(long folderId);
        Task AddNewFolderAsync(Folder folder);
        Task UpdateFolderAsync(Folder folder);
        Task DeleteFolderAsync(long folderId);

        Task<IEnumerable<Models.Folder>> GetRootSubFolderAsync(int assetId);
        Task<IEnumerable<Models.Folder>> GetSubFolderAsync(int assetId, long? parentFolderId);
        Task<List<Models.Folder>> GetFolderPath(long? folderId);
    }
}
