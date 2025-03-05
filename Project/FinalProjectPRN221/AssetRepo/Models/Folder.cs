using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetRepo.Models
{
    public partial class Folder
    {
        public Folder()
        {
            Files = new HashSet<File>();
        }

        public long FolderId { get; set; }

        [Required(ErrorMessage = "Please input folder name")]
        public string? FolderName { get; set; }
        public int? ParentFolderId { get; set; }
        public int? AssetId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Asset? Asset { get; set; }
        public virtual ICollection<File> Files { get; set; }
    }
}
