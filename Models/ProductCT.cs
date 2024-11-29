namespace WBMT.Models
{
    public class ProductCT
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
        //	public Brand Brand { get; set; }
        // Foreign Key to Category
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        //	public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
