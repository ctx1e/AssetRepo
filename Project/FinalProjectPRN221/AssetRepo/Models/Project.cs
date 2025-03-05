using System;
using System.Collections.Generic;

namespace AssetRepo.Models
{
    public partial class Project
    {
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? AssetId { get; set; }

        public virtual Asset? Asset { get; set; }
    }
}
