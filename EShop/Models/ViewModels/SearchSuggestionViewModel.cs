namespace Models.ViewModels
{
    public class SearchSuggestionViewModel
    {
        public string ProductName { get; set; } = null!;

        public int ProductId { get; set; }

        public string? SubCategory { get; set; }
    }
}
