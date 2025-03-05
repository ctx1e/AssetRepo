using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetRepo.Models
{
    public partial class Asset
    {
        public Asset()
        {
            Files = new HashSet<File>();
            Folders = new HashSet<Folder>();
            Projects = new HashSet<Project>();
        }

        public int AssetId { get; set; }

        [Required(ErrorMessage = "Please input asset name!")]
        public string? AssetName { get; set; }


        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual AssetSold? AssetSold { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
