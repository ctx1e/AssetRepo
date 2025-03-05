using AssetRepo.Models;
using AssetRepo.ModelView;
using AssetRepo.Pattern.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AssetRepo.Pages.Assets
{
    public class AssetDetailModel : PageModel
    {
        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;
        private readonly IAssetService _assetService;
        public AssetDetailModel(IFolderService folderService, IFileService fileService, IAssetService assetService)
        {
            _folderService = folderService;
            _fileService = fileService;
            _assetService = assetService;
        }

        [BindProperty]
        public int aid { get; set; }

        [BindProperty]
        public long? folderId { get; set; }
        public AssetDetailViewModel AssetDetail { get; set; }

        public async Task OnGetAsync(int aid, long? folderId)
        {
            
                TempData["AssetId"] = aid;
            
                TempData["FolderId"] = folderId;

            Folder currentFolder;
            IEnumerable<Folder> subFolders;
            IEnumerable<Models.File> files;

            if (folderId == null)
            {
                // Lấy các thư mục gốc của asset (ParentFolderId = null) và file ở cấp độ gốc của asset đó (FolderId = null)
                subFolders = await _folderService.GetRootSubFolderAsync(aid);
                files = await _fileService.GetAllRootFile(aid); 
                currentFolder = null; // Không có thư mục cụ thể nào
            }
            else
            {
                currentFolder = await _folderService.GetFolderByIdAsync(folderId.Value);
                subFolders = await _folderService.GetSubFolderAsync(aid, folderId.Value);
                files = await _fileService.GetAllFileByFolderId(aid, folderId.Value);
            }

            var assetById = await _assetService.GetAssetByIdAsync(aid);

            AssetDetail = new AssetDetailViewModel
            {
                AssetId = assetById.AssetId,
                AssetName = assetById.AssetName,
                CurrentFolder = currentFolder,
                SubFolders = subFolders,
                Files = files
            };
        }


        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
        }


        public async Task<List<Folder>> GetFolderPath(long? folderId)
        {
            List<Folder> path = new List<Folder>();

            path = await _folderService.GetFolderPath(folderId.Value);
            return path;
        }
    }
}
