using AssetRepo.Models;
using AssetRepo.Pattern.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AssetRepo.Pages.Assets
{
    public class UploadModel : PageModel
    {

        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;
        private readonly IAssetService _assetService;
        private readonly IWebHostEnvironment _env;


        public UploadModel(IFolderService folderService, IFileService fileService, IAssetService assetService, IWebHostEnvironment env)
        {
            _folderService = folderService;
            _fileService = fileService;
            _assetService = assetService;
            _env = env;

        }

        [BindProperty]
        public int aid { get; set; }

        [BindProperty]
        public long? folderId { get; set; }

        [BindProperty]
        public string type { get; set; }

        [BindProperty]
        public Models.Folder folder { get; set; }

        [BindProperty]
        public Models.File file { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please input file!")]
        public IFormFile UploadedFile { get; set; }

        [BindProperty]
        public string typeName { get; set; }
        public async Task OnGetAsync(int aid, long? folderId, string type)
        {
            if (type.ToLower().Equals("newfolder"))
            {
                folder = new Models.Folder();
                typeName = "New Folder";
            }
            else
            {
                typeName = "Upload File";
            }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (typeName.ToLower().Equals("new folder"))
            {
                var existingFolder = await _folderService.GetFolderByIdAsync(folderId.Value);
                IEnumerable<Models.Folder> allSubFolders = null;
                IEnumerable<Models.File> allFilesInFolder;

                if (existingFolder != null)
                {
                    if (existingFolder.FolderId != null)
                    {
                        // Lấy danh sách thư mục con và tệp trong thư mục hiện tại
                        allSubFolders = await _folderService.GetSubFolderAsync(existingFolder.AssetId.Value, existingFolder.FolderId);
                        allFilesInFolder = await _fileService.GetAllFileByFolderId(aid, existingFolder.FolderId); // Đảm bảo dùng FolderId đúng
                    }
                    else
                    {
                        allSubFolders = await _folderService.GetRootSubFolderAsync(existingFolder.AssetId.Value);
                        allFilesInFolder = await _fileService.GetAllRootFile(aid);
                    }

                    // Kiểm tra tên thư mục đã tồn tại
                    if (allSubFolders.Any(f => f.FolderName == folder.FolderName) || allFilesInFolder.Any(f => f.FileName == folder.FolderName))
                    {
                        TempData["Message"] = "The folder name already existed!";
                        TempData["MessageType"] = "error";
                        TempData["AssetId"] = aid;
                        TempData["FolderId"] = folderId;
                        //TempData["Folderr"] = existingFolder;

                        return Page();
                    }
                }
                else
                {
                    allSubFolders = await _folderService.GetRootSubFolderAsync(aid);
                    allFilesInFolder = await _fileService.GetAllRootFile(aid);
                    if (allSubFolders.Any(f => f.FolderName == folder.FolderName) || allFilesInFolder.Any(f => f.FileName == folder.FolderName))
                    {
                        TempData["Message"] = "The folder name already existed!";
                        TempData["MessageType"] = "error";
                        TempData["AssetId"] = aid;
                        TempData["FolderId"] = folderId;
                        //TempData["Folderr"] = existingFolder;

                        return Page();
                    }
                }
        
                if (folderId == 0 || folderId == null)
                {
                    folder.ParentFolderId = null;
                }
                else
                {
                    folder.ParentFolderId = (int)folderId;

                }
                folder.AssetId = aid;
                folder.CreatedDate = DateTime.Now;
                await _folderService.AddNewFolderAsync(folder);
                TempData["Message"] = $"Rename Folder '{folder.FolderName}' success";
                TempData["MessageType"] = "success";

                if (folder.ParentFolderId == null)
                {
                    return RedirectToPage("/Assets/AssetDetail", new { aid = aid });
                }
                else
                {
                    return RedirectToPage("/Assets/AssetDetail", new { aid = this.aid, folderId = folder.ParentFolderId });
                }

            } else
            {
                if(UploadedFile != null && UploadedFile.Length > 0)
                {

                    // Ktra Trung lap file name
                    var existingFolder = await _folderService.GetFolderByIdAsync(folderId.Value);
                    IEnumerable<Models.Folder> allSubFolders = null;
                    IEnumerable<Models.File> allFilesInFolder;

                    if (existingFolder != null)
                    {
                        if (existingFolder.FolderId != null)
                        {
                            // Lấy danh sách thư mục con và tệp trong thư mục hiện tại
                            allSubFolders = await _folderService.GetSubFolderAsync(existingFolder.AssetId.Value, existingFolder.FolderId);
                            allFilesInFolder = await _fileService.GetAllFileByFolderId(aid, existingFolder.FolderId);
                        }
                        else
                        {
                            allSubFolders = await _folderService.GetRootSubFolderAsync(existingFolder.AssetId.Value);
                            allFilesInFolder = await _fileService.GetAllRootFile(aid);
                        }

                        // Kiểm tra tên thư mục đã tồn tại
                        if (allSubFolders.Any(f => f.FolderName == file.FileName) || allFilesInFolder.Any(f => f.FileName == file.FileName))
                        {
                            TempData["Message"] = "The file name already existed!";
                            TempData["MessageType"] = "error";
                            TempData["AssetId"] = aid;
                            TempData["FolderId"] = folderId;
                            //TempData["Folderr"] = existingFolder;

                            return Page();
                        }
                    }
                    else
                    {
                        allSubFolders = await _folderService.GetRootSubFolderAsync(aid);
                        allFilesInFolder = await _fileService.GetAllRootFile(aid);
                        if (allSubFolders.Any(f => f.FolderName == file.FileName) || allFilesInFolder.Any(f => f.FileName == file.FileName))
                        {
                            TempData["Message"] = "The file name already existed!";
                            TempData["MessageType"] = "error";
                            TempData["AssetId"] = aid;
                            TempData["FolderId"] = folderId;
                            //TempData["Folderr"] = existingFolder;

                            return Page();
                        }
                    }

                    // lay ra ten cua file duoc tai len
                    
                    
                    var assetById = await _assetService.GetAssetByIdAsync(aid);
                    // tao duong dan voi folder theo assetId va folderId
                    string folderName = (folderId == 0 || folderId == null) ? "root" : folderId.ToString();
                    string folderPath = Path.Combine("https://localhost:7077",_env.WebRootPath, "upload", assetById.AssetName, folderName);
                    

                    // lay duoi cua file
                    string fileExtension = Path.GetExtension(UploadedFile.FileName);
                    if (!Directory.Exists(folderPath)) { 
                    Directory.CreateDirectory(folderPath);
                    }

                    string fileName = file.FileName + fileExtension;
                    string fullPath = Path.Combine(folderPath, fileName);
                    using(var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await UploadedFile.CopyToAsync(stream);
                    }

                   

                    if (folderId == null || folderId == 0)
                    {
                        file.FolderId = null;
                    } else
                    {
                        file.FolderId = folderId;
                    }
                    file.AssetId = aid;
                    file.OriginalFileName = fileName;
                    file.TypeFile = DetermineFileType(fileExtension);
                    file.FilePath = fullPath;
                    file.CreatedDate = DateTime.Now;

                    await _fileService.AddFileAsync(file);
                    TempData["Message"] = $"Upload  File '{file.FileName}' success";
                    TempData["MessageType"] = "success";

                    if (file.FolderId == null)
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

        private string DetermineFileType(string extension)
        {
            // Định nghĩa các trường hợp cho từng loại tệp
            switch (extension.ToLower())
            {
                // Img
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".bmp":
                case ".tga":
                case ".psd":
                case ".svg":
                    return "File Image";

                // Audio
                case ".wav":
                case ".mp3":
                case ".ogg":
                case ".flac":
                case ".aac":
                case ".m4a":
                    return "File Audio";

                // Video
                case ".mp4":
                case ".mov":
                case ".avi":
                case ".mkv":
                case ".webm":
                    return "File Video";

                // Script/Coding
                case ".cs":   // C# Script
                case ".js":   // JavaScript
                case ".lua":  // Lua Script
                case ".py":   // Python Script
                case ".json": // Config/Settings
                case ".xml":  // Config/Settings
                case ".shader":  // Shader file for rendering
                    return "File Script";

                // Animation
                case ".anim":     // Unity Animation file
                case ".fbx":      // Animation or 3D model with animation
                case ".dae":      // COLLADA format for animations
                case ".blend":    // Blender file (can contain animations)
                case ".bvh":      // Motion capture animation
                    return "File Animation";

                // 3D Models
                case ".obj":
                case ".gltf":
                case ".glb":
                case ".3ds":
                case ".max":
                case ".c4d":
                    return "File 3D Model";

                // Prefab
                case ".prefab":   // Unity Prefab file for reusable objects
                case ".asset":    // Unity Asset file (can represent various prefab resources)
                    return "File Prefab";

                case ".unity":  // Unity Scene file
                case ".scene":  // Generic scene file extension, e.g., used in some engines
                case ".lvl":    // Level file, có thể dùng trong game engines khác nhau
                case ".umap":   // Unreal Engine map/scene file
                case ".t3d":    // Unreal Engine 3D scene file
                    return "File Scene";

                // Textures/Materials
                case ".mat":  // Material file
                case ".sbsar":  // Substance file for procedural textures
                case ".dds":  // DirectDraw Surface (common for textures)
                    return "File Texture";

                case ".inputactions":
                    return "File Input System";

                // Other file
                case ".txt":
                case ".md":
                case ".pdf":
                case ".doc":
                case ".docx":
                    return "File Document";

                default:
                    return "File Other"; 
            }
        }

    }
}
