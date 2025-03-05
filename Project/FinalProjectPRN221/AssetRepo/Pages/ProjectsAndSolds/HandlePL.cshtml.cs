using AssetRepo.Models;
using AssetRepo.Pattern.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace AssetRepo.Pages.ProjectsAndSolds
{
    public class HandlePLModel : PageModel
    {
        private readonly IProjectService _projectService;
        private readonly IAssetService _assetService;
        [BindProperty(SupportsGet = true)]
        public int? pid { get; set; }
        

        [BindProperty]
        public Project Project { get; set; } 

        public IEnumerable<Asset> AvailableAssets { get; set; } 

        public HandlePLModel(IProjectService projectService, IAssetService assetService)
        {
            _projectService = projectService;
            _assetService = assetService;
        }

        public async Task OnGetAsync()
        {
            Project = pid.HasValue ? await _projectService.GetProjectByIdAsync(pid.Value) : new Project();
            AvailableAssets = await _assetService.GetAvailableAssetsForProjectAsync();
        }

        public async Task<IActionResult> OnPost()
        {
            
            try
            {
                if (Project.ProjectId == 0)
                {
                    Project.CreatedDate = DateTime.Now;
                    await _projectService.AddProjectAsync(Project);
                    TempData["Message"] = $"Add new Asset '{Project.ProjectName}' in project repository success";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    await _projectService.UpdateProjectAsync(Project);
                    TempData["Message"] = $"Update asset project '{Project.ProjectName}' success";
                    TempData["MessageType"] = "success";
                }
      
            }
            catch (Exception)
            {
                TempData["Message"] = "Something wrong! Please try again";
                TempData["MessageType"] = "error";
            }
            return RedirectToPage("/ProjectsAndSolds/PAS");
        }
        public async Task<IActionResult> OnGetDelete()
        {
            Models.Project project = await _projectService.GetProjectByIdAsync(pid.Value);
            if (project != null)
            {
                await _projectService.DeleteProjectAsync(project.ProjectId);
                TempData["Message"] = $"Delete product '{project.ProjectName}' from Project Repository success";
                TempData["MessageType"] = "success";
            }
            else
            {
                TempData["Message"] = "No asset found to delete";
                TempData["MessageType"] = "error";
            }
            return RedirectToPage("/ProjectsAndSolds/PAS");

        }

    }
}
