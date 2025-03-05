namespace AssetRepo.Pages.Shared.Pagination
{
    public class PaginationHelper
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public string CurrentPageHandler { get; set; }
        public string SearchTerm { get; set; }
        public PaginationHelper(int totalItems, int pageSize, int currentPage, string currentPageHandler, string searchTerm = null)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            CurrentPageHandler = currentPageHandler;
            SearchTerm = searchTerm;
        }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public int PreviousPage => HasPreviousPage ? CurrentPage - 1 : 1;
        public int NextPage => HasNextPage ? CurrentPage + 1 : TotalPages;

        public string GetPageUrl(int page)
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                return $"{CurrentPageHandler}/{page}/{SearchTerm}";
            }
            return $"{CurrentPageHandler}/{page}";
        }
    }


}
