namespace Models.ViewModels
{
    public class FavoriteReportProductViewModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = null!;
      
        public decimal Price { get; set; }

        public byte[]? LargePhoto { get; set; }

    }
}
