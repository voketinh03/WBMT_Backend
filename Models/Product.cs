using System.ComponentModel.DataAnnotations;

namespace WBMT.Models
{
    public class Product
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        [Url(ErrorMessage = "Image URL must be a valid URL.")]
        public string? ImageUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Brand ID must be greater than 0.")]
        public int? BrandId { get; set; }
        public int ProductId { get; internal set; }
    }
}

