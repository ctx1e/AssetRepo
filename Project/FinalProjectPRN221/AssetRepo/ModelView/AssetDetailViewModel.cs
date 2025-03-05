using AssetRepo.Models;

namespace AssetRepo.ModelView
{
    public class AssetDetailViewModel

    {
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public Models.Folder CurrentFolder { get; set; }
        public IEnumerable<Models.Folder> SubFolders { get; set; }
        public IEnumerable<Models.File> Files { get; set; }
    }
}
