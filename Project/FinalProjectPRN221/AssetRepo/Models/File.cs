using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetRepo.Models
{
    public partial class File
    {
        public int FileId { get; set; }
        public long? FolderId { get; set; }
        public int? AssetId { get; set; }

        [Required(ErrorMessage = "Please input file name!")]
        public string? FileName { get; set; }
        public string? OriginalFileName { get; set; }
        public string? TypeFile { get; set; }
        public string? FilePath { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Asset? Asset { get; set; }
        public virtual Folder? Folder { get; set; }
    }
}
