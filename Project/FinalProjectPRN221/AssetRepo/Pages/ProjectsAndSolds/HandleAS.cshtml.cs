using AssetRepo.Models;
using AssetRepo.Pattern.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace AssetRepo.Pages.ProjectsAndSolds
{
    public class HandleASModel : PageModel
    {
        private readonly IAssetSoldService _assetSoldService;
        private readonly IAssetService _assetService;

        [BindProperty]
        public AssetSold AssetSold { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? asid { get; set; }
        public IEnumerable<Asset> AvailableAssets { get; set; }

        public HandleASModel(IAssetSoldService assetSoldService, IAssetService assetService)
        {
            _assetSoldService = assetSoldService;
            _assetService = assetService;
        }

        public async Task OnGetAsync()
        {

            AssetSold = asid.HasValue ? await _assetSoldService.GetSoldAssetByIdAsync(asid.Value) : new AssetSold();

            int? currentAssetId = AssetSold.AssetId > 0 ? AssetSold.AssetId : null;

            AvailableAssets = await _assetService.GetAvailableAssetsForSaleAsync(currentAssetId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                if (AssetSold.AssetSoldId == 0)
                {

                    await _assetSoldService.AddSoldAssetAsync(AssetSold);
                    TempData["Message"] = $"Add new asset in Asset Sold success";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    await _assetSoldService.UpdateSoldAssetAsync(AssetSold);
                    TempData["Message"] = $"Update asset success";
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
            Models.AssetSold assetSold = await _assetSoldService.GetSoldAssetByIdAsync(asid.Value);
            if (assetSold != null)
            {
                await _assetSoldService.DeleteSoldAssetAsync(assetSold.AssetSoldId);
                TempData["Message"] = $"Delete asset '{assetSold.Asset.AssetName}' from Project Repository success";
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
