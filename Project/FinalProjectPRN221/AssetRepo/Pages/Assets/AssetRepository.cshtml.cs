using AssetRepo.Models;
using AssetRepo.Pages.Shared.Pagination;
using AssetRepo.Pattern.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssetRepo.Pages.Assets
{
    public class AssetRepositoryModel : PageModel
    {
        private readonly IAssetService _assetService;

        public IEnumerable<Asset>? Assets { get; private set; }
        public PaginationHelper Pagination { get; private set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        private const int PageSize = 5;

        [BindProperty]
        public int TotalAssets { get; private set; }


        public AssetRepositoryModel(IAssetService assetService)
        {
            _assetService = assetService;
        }

        public async Task OnGetAsync()
        {
            await LoadAssetsAsync();
        }

        private async Task LoadAssetsAsync()
        {
            TotalAssets = await _assetService.GetAssetCountAsync();
            Assets = await _assetService.GetAllAssetsAsync(PageNumber, PageSize);
            Pagination = new PaginationHelper(TotalAssets, PageSize, PageNumber, "/Assets/AssetRepository/List");
        }

        public async Task<IActionResult> OnGetSearch(string searchTerm)
        {
            SearchTerm = searchTerm;
   
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                var rs = await _assetService.SearchAssetsByNameAsync(SearchTerm, PageNumber, PageSize); ;
                TotalAssets = rs.TotalCount;
                Assets =   rs.Assets;
                Pagination = new PaginationHelper(TotalAssets, PageSize, PageNumber, "/Assets/AssetRepository/Search", searchTerm);
            }
            else
            {
                await LoadAssetsAsync();
            }

            return Page();
        }
    }
}
