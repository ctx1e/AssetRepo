namespace AssetRepo.Pages.Shared
{
    public class NotifyError : Exception
    {

        public NotifyError(string message) : base(message) { }
    }
}
