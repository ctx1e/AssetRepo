using AssetRepo.Pattern.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AssetRepo.Pages.Assets
{
    public class DisplayFileModel : PageModel
    {

        private readonly IFileService _fileService;
        public string FileContent { get; set; }
        public int fid { get; set; }
        public Models.File fileById { get; set; }
        public DisplayFileModel(IFileService fileService)
        {
            _fileService = fileService;
        }
        public async Task<IActionResult> OnGet(int fid, string type)
        {
            fileById = await _fileService.GetFileById(fid);
            if(type.ToLower().Equals("display"))
            {
                var file = await _fileService.GetFileById(fid);
                if (file == null) return NotFound();

                if (System.IO.File.Exists(file.FilePath))
                {
                    FileContent = System.IO.File.ReadAllText(file.FilePath); // Read content of file
                }
                else
                {
                    FileContent = "File not found.";
                }

                return Page();
            } else
            {
                var file = await _fileService.GetFileById(fid);
                if (file == null) return NotFound();
                

                // Tạo đường dẫn tuyệt đối đến file
                var path = file.FilePath.TrimStart('/');

                // Kiểm tra file có tồn tại không
                if (!System.IO.File.Exists(file.FilePath))
                {
                    return NotFound("File not found on the server.");
                }

                // Đọc nội dung file vào stream và trả về FileResult để tải xuống
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                
                return File(memory, "application/octet-stream", Path.GetFileName(path));
            }
            
        }
    }
}
