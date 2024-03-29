﻿using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ShoppingCartItemViewModel
    {
        public int ShoppingCartItemId { get; set; }

        [Required]
        public string ShoppingCartId { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; } = null!;

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal ProductPrice { get; set; }

        [Required]
        [DataType("Integer")]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        [DataType("float")]
        public decimal? DiscountPcnt { get; set; }

        [Display(Name = "Total + discount")]
        [DataType(DataType.Currency)]
        public decimal Total => Math.Round((decimal)(ProductPrice * (1 - DiscountPcnt) * Quantity), 2);

        public int? ProductPhotoId { get; set; }
    }
}