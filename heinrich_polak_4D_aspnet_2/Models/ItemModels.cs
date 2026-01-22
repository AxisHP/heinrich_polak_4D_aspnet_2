using System;
using System.ComponentModel.DataAnnotations;

namespace heinrich_polak_4D_aspnet_2.Models
{
    public class CreateItemModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be 0 or greater")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }
    }

    public class UpdateItemModel
    {
        public Guid PublicId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be 0 or greater")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }
    }

    public class DeleteItemModel
    {
        public Guid PublicId { get; set; }
        public string Name { get; set; }
    }
}
