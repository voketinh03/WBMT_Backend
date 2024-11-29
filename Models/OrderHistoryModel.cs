namespace WBMT.Models
{
    public class OrderHistoryModel
    {
        public string IdOrder { get; set; }           // ID ĐH - Mã đơn hàng
        public string Id { get; set; }                // ID KH - Mã khách hàng
        public DateTime OrderDate { get; set; }        // Ngày mua
        public string ProductName { get; set; }        // Sản phẩm
        public int Quantity { get; set; }              // Số lượng
        public decimal Price { get; set; }             // Thành tiền (giá của từng sản phẩm)
        public string OrderStatus { get; set; }        // Trạng thái

        

       
    }
}
