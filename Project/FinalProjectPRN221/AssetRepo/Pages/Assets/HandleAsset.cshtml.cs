using AssetRepo.Models;
using AssetRepo.Pages.Shared;
using AssetRepo.Pattern.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace AssetRepo.Pages.Assets
{
    public class HandleAssetModel : PageModel
    {

        private readonly IAssetService _assetService;

        [BindProperty]
        public Models.Asset Asset { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? aid { get; set; }

        public HandleAssetModel(IAssetService assetService)
        {
            _assetService = assetService;
        }
        public async Task OnGet()
        {
            if(!aid.HasValue)
            {
                Asset = new Models.Asset();
            }
        }

        public async Task OnGetUpdate()
        {
            Asset = await _assetService.GetAssetByIdAsync(aid.Value);
            if (Asset == null)
            {
                ViewData["ErrorAid"] = "Cannot Find Asset";
            } 
        }

        public async Task<IActionResult> OnPost()
        {
            
            
            
                if (Asset.AssetId == 0) // add new asset
                {
                    bool isNameAssetExist = await _assetService.IsNameAssetExistAsync(Asset);
                    if (isNameAssetExist)
                    {
                        TempData["Message"] = $"Asset with the name '{Asset.AssetName}' already exists! Enter again";
                        TempData["MessageType"] = "error";
                        return Page();
                    }
                    Asset.CreatedDate = DateTime.Now;
                    await _assetService.AddAssetAsync(Asset);
                    TempData["Message"] = $"Add Asset '{Asset.AssetName}' success";
                    TempData["MessageType"] = "success";
                }
                else 
                {
                    var existingAsset = await _assetService.GetAssetByIdAsync(Asset.AssetId);
                    bool isNameAssetExist = await _assetService.IsNameAssetExistAsync(Asset);
                    if (isNameAssetExist)
                    {
                        TempData["Message"] = $"Asset with the name '{Asset.AssetName}' already exists! Enter again";
                        TempData["MessageType"] = "error";
                        return Page();
                    }
                    
                        existingAsset.AssetName = Asset.AssetName;
                        existingAsset.Description = Asset.Description;

                                    await _assetService.UpdateAssetAsync(existingAsset);
                    TempData["Message"] = $"Update asset '{Asset.AssetName}' success";
                    TempData["MessageType"] = "success";
                }
            
            

            return RedirectToPage("/Assets/AssetRepository");
        }


        public async Task<IActionResult> OnGetDelete()
        {
            Models.Asset asset = await _assetService.GetAssetByIdAsync(aid.Value);
            if (asset != null)
            {
                await _assetService.DeleteAssetAsync(asset.AssetId);
                TempData["Message"] = $"Delete product '{asset.AssetName}' success";
                TempData["MessageType"] = "success";
            }
            else
            {
                TempData["Message"] = "No asset found to delete";
                TempData["MessageType"] = "error";
            }
            return RedirectToPage("/Assets/AssetRepository");

        }
    }
}
