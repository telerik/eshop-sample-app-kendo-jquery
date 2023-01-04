namespace Models.ViewModels
{
    public class ProductAllViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ProductNumber { get; set; } = null!;

        public int? ModelId { get; set; }

        public string? Model { get; set; }

        public string? Color { get; set; }

        public decimal? Price { get; set; }

        public string? Size { get; set; }

        public decimal? Weight { get; set; }

        public decimal? DiscountPct { get; set; }

        public decimal? FinalPrice => Price * (1 - DiscountPct);

        public int? ProductCategoryId { get; set; }
        public string? ProductCategoryName { get; set; }

        public int? SubCategoryId { get; set; }

        public string? SubCategory { get; set; }

        public double? AverageRating { get; set; }

        public int? PhotoId { get; set; }

        public byte[]? ThumbNailPhoto { get; set; }
        public byte[]? LargePhoto { get; set; }

        public string? Description { get; set; }
        public short? Quantity { get; set; }
    }
}