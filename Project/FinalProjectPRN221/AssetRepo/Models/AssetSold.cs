using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetRepo.Models
{
    public partial class AssetSold
    {
        public int AssetSoldId { get; set; }

        [Required(ErrorMessage = "Please select an asset.")]
        public int? AssetId { get; set; }
        public decimal? SalePrice { get; set; }

        public virtual Asset? Asset { get; set; }
        public string FormattedSalePrice
        {
            get
            {

                return SalePrice.HasValue
                    ? SalePrice.Value.ToString("N0") : "No Price Yet";
            }
        }

    }

}
