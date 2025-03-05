using AssetRepo.Models;
using AssetRepo.Pattern.Repository;
using AssetRepo.Pattern.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography.X509Certificates;

namespace AssetRepo.Pages.Assets
{
    public class HandleRenameModel : PageModel
    {
        //private readonly AssetRepoContext _context;

        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;

        public HandleRenameModel(IFolderService folderService, IFileService fileService, AssetRepoContext context)
        {
            _folderService = folderService;
            _fileService = fileService;
            //_context = context;
        }


        [BindProperty]
        public long? folderId { get; set; }

        [BindProperty]
        public int? fileId { get; set; }

        [BindProperty]
        public int aid { get; set; }

        [BindProperty]
        public string type { get; set; }

        [BindProperty]
        public Models.Folder Folderr { get; set; }

        [BindProperty]
        public Models.File Filee { get; set; }

        [BindProperty]
        public string typeName { get; set; }

        public int AssetId { get; set; }
        public long? FolderId { get; set; }

        public async Task OnGet(long? folderId, int? fileId, int aid, string type)
        {
            await OnGetCheckTypeAsync(folderId, fileId, aid, type);
        }

        public async Task OnGetCheckTypeAsync(long? folderId, int? fileId, int aid, string type)
        {
            AssetId = aid;
            FolderId = folderId;
            Folderr = await _folderService.GetFolderByIdAsync(folderId.Value);
            Filee = await _fileService.GetFileById(fileId.Value);
            if (type.ToLower().Equals("folder"))
            {
                typeName = "Rename Folder";

            }
            else if (type.ToLower().Equals("file"))
            {
                typeName = "Rename File";
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (type.ToLower().Equals("folder"))
            {
                var existingFolder = await _folderService.GetFolderByIdAsync(folderId.Value);
                IEnumerable<Models.Folder> allSubFolders = null;
                IEnumerable<Models.File> allFilesInFolder;
                if (existingFolder != null)
                {
                    if (existingFolder.ParentFolderId.HasValue)
                    {
                        allSubFolders = await _folderService.GetSubFolderAsync(existingFolder.AssetId.Value, existingFolder.ParentFolderId.Value);
                        allFilesInFolder = await _fileService.GetAllFileByFolderId(aid, existingFolder.ParentFolderId);
                    }
                    else
                    {
                        allSubFolders = await _folderService.GetRootSubFolderAsync(existingFolder.AssetId.Value);
                        allFilesInFolder = await _fileService.GetAllRootFile(aid);

                    }
                    if (allSubFolders.Any(f => f.FolderName == Folderr.FolderName) || allFilesInFolder.Any(f => f.FileName == Folderr.FolderName))
                    {
                        TempData["Message"] = "The folder name already existed!";
                        TempData["MessageType"] = "error";
                        TempData["AssetId"] = aid;
                        TempData["FolderId"] = folderId;
                        TempData["Folderr"] = existingFolder; 
                        TempData["Filee"] = Filee;
                        return Page();
                    }

                    existingFolder.FolderName = Folderr.FolderName;
                    await _folderService.UpdateFolderAsync(existingFolder);
                    TempData["Message"] = $"Rename Folder '{existingFolder.FolderName}' success";
                    TempData["MessageType"] = "success";

                    if (existingFolder.ParentFolderId == null)
                    {
                        return RedirectToPage("/Assets/AssetDetail", new { aid = aid });
                    }
                    else
                    {
                        return RedirectToPage("/Assets/AssetDetail", new { aid = this.aid, folderId = existingFolder.ParentFolderId });
                    }
                }
            }
            else
            {
                Models.File existingFile = await _fileService.GetFileById(fileId.Value);
                IEnumerable<Models.File> allFilesInFolder;
                IEnumerable<Models.Folder> allSubFolders = null;
                if (existingFile != null)
                {
                    if (existingFile.FolderId.HasValue)
                    {
                        allFilesInFolder = await _fileService.GetAllFileByFolderId(aid, existingFile.FolderId.Value);
                        allSubFolders = await _folderService.GetSubFolderAsync(existingFile.AssetId.Value, existingFile.FolderId);
                    }
                    else
                    {
                        allFilesInFolder = await _fileService.GetAllRootFile(aid);
                        allSubFolders = await _folderService.GetRootSubFolderAsync(existingFile.AssetId.Value);

                    }
                    if (allFilesInFolder.Any(f => f.FileName == Filee.FileName) || allSubFolders.Any(f => f.FolderName == Filee.FileName))
                    {
                        TempData["Message"] = "The file name already existed!";
                        TempData["MessageType"] = "error";
                        TempData["AssetId"] = aid;
                        TempData["FolderId"] = folderId;
                        TempData["Folderr"] = Folderr;
                        TempData["Filee"] = existingFile;
                        return Page();
                    }

                    existingFile.FileName = Filee.FileName;
                    await _fileService.UpdateFileAsync(existingFile);
                    TempData["Message"] = $"Rename Folder '{Filee.FileName}' success";
                    TempData["MessageType"] = "success";

                    if (existingFile.FolderId == null)
                    {
                        return RedirectToPage("/Assets/AssetDetail", new { aid = this.aid });
                    }
                    else
                    {
                        return RedirectToPage("/Assets/AssetDetail", new { aid = this.aid, folderId = this.folderId });
                    }
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(long? folderId, int? fileId, int aid, string type)
        {


            if (type.ToLower().Equals("folder"))
            {
                var folderById = await _folderService.GetFolderByIdAsync(folderId.Value);
                await _folderService.DeleteFolderAsync(folderId.Value);
                TempData["Message"] = $"Delete Folder '{folderById.FolderName}' success";
                TempData["MessageType"] = "success";
                if (folderById.ParentFolderId == null)
                {
                    return RedirectToPage("/Assets/AssetDetail", new { aid = aid });
                }
                else
                {
                    return RedirectToPage("/Assets/AssetDetail", new { aid = aid, folderId = folderById.ParentFolderId });
                }
            }
            else
            {
                var fileById = await _fileService.GetFileById(fileId.Value);
                await _fileService.DeleteFileAsync(fileId.Value);
                TempData["Message"] = $"Delete Folder '{fileById.FileName}' success";
                TempData["MessageType"] = "success";
                if (fileById.FolderId == null)
                {
                    return RedirectToPage("/Assets/AssetDetail", new { aid = aid });
                }
                else
                {
                    return RedirectToPage("/Assets/AssetDetail", new { aid = aid, folderId = fileById.FolderId });
                }
            }
            return Page();
        }
    }
}
