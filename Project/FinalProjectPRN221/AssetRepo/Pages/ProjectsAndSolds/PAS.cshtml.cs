using AssetRepo.Models;
using AssetRepo.Pages.Shared.Pagination;
using AssetRepo.Pattern.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetRepo.Pages.ProjectsAndSolds
{
    public class PASModel : PageModel
    {
        private readonly IProjectService _projectService;
        private readonly IAssetSoldService _assetSoldService;

        [BindProperty]
        public IEnumerable<AssetSold>? SoldAssets { get; set; }
        [BindProperty]
        public IEnumerable<Project>? Projects { get; set; } 

        public PaginationHelper ProjectPagination { get; set; }
        public PaginationHelper SoldAssetPagination { get; set; }

        [BindProperty(SupportsGet = true)]
        public int pageNumber { get; set; }

        public int totalProjects { get; set; }
        public int totalSoldAssets { get; set; }

        public PASModel(IProjectService projectService, IAssetSoldService assetSoldService)
        {
            _projectService = projectService;
            _assetSoldService = assetSoldService;
        }

        public void OnGetAsync()
        {
        }

        // Load Project Data
        public async Task OnGetPLAsync(int pageSize = 5)
        {
            totalProjects = await _projectService.GetProjectCountAsync();
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            Projects = await _projectService.GetAllProjectsAsync(pageNumber, pageSize);
            ProjectPagination = new PaginationHelper(totalProjects, pageSize, pageNumber, "/ProjectsAndSolds/PAS/PL");
        }

        // Load Sold Asset Data
        public async Task OnGetASAsync(int pageSize = 5)
        {
            totalSoldAssets = await _assetSoldService.GetSoldAssetCountAsync();
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            SoldAssets = await _assetSoldService.GetAllSoldAssetsAsync(pageNumber, pageSize);
            SoldAssetPagination = new PaginationHelper(totalSoldAssets, pageSize, pageNumber, "/ProjectsAndSolds/PAS/AS");
        }
    }
}
