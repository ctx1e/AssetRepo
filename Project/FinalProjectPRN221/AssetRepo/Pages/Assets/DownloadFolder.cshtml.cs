using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO.Compression;

namespace AssetRepo.Pages.Assets
{
    public class DownloadFolderModel : PageModel
    {
        public async  Task<IActionResult> OnGet(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                return NotFound("Folder not found.");
            }

            // ???ng d?n file zip t?m th?i
            var zipFilePath = Path.Combine(Path.GetTempPath(), $"{Path.GetFileName(folderPath)}.zip");

            // Nén th? m?c
            ZipFile.CreateFromDirectory(folderPath, zipFilePath);

            // ??c file zip vào MemoryStream ?? tr? v?
            var memory = new MemoryStream();
            using (var stream = new FileStream(zipFilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            // Xóa file zip t?m th?i sau khi ?ã n?p vào MemoryStream
            System.IO.File.Delete(zipFilePath);

            return File(memory, "application/zip", $"{Path.GetFileName(folderPath)}.zip");
        }
    }
}
