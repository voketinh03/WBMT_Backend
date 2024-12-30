using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WBMT.Data;
using WBMT.Model;
using WBMT.Models;

namespace WBMT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
       		private readonly string _connectionString;

		public ProductsController(IConfiguration configuration)
		{
			// Lấy chuỗi kết nối từ appsettings.json
			_connectionString = configuration.GetConnectionString("WBMT");
		}

	    // GET: api/Products
		[HttpGet]
		public IActionResult GetAllProducts()
		{
			List<ProductCT> productcts = new List<ProductCT>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				string query = @"
		SELECT p.product_id, p.product_name, p.brand_id, p.category_id, p.price, p.quantity, p.image_url,
			b.brand_name, c.category_name, c.description AS category_description
		FROM Products p
		JOIN Brands b ON p.brand_id = b.brand_id
		JOIN Categories c ON p.category_id = c.category_id";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						var productct = new ProductCT
						{
							ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
							ProductName = reader.GetString(reader.GetOrdinal("product_name")),
							BrandId = reader.GetInt32(reader.GetOrdinal("brand_id")),
							BrandName = reader.GetString(reader.GetOrdinal("brand_name")),
							CategoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
							CategoryName = reader.GetString(reader.GetOrdinal("category_name")),
							Description = reader.GetString(reader.GetOrdinal("category_description")),
							Price = reader.GetDecimal(reader.GetOrdinal("price")),
							Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
							ImageUrl = reader.GetString(reader.GetOrdinal("image_url")), // lấy URL ảnh
						};

						productcts.Add(productct);
					}
				}
			}

			if (productcts.Count == 0)
			{
				return NotFound("No products found.");
			}

			return Ok(new { status = "success", data = productcts });
		}

		//


		// GET: api/Products/5
		[HttpGet("{id}")]
		public IActionResult GetProductById(int id)
		{
			ProductCT productct = null;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				// Truy vấn một sản phẩm theo id kết hợp với bảng Brands và Categories
				string query = @"
		SELECT p.product_id, p.product_name, p.brand_id, p.category_id, p.price, p.quantity, p.image_url,
			b.brand_name, c.category_name, c.description AS category_description
		FROM Products p
		JOIN Brands b ON p.brand_id = b.brand_id
		JOIN Categories c ON p.category_id = c.category_id
		WHERE p.product_id = @ProductId";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@ProductId", id);

					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							productct = new ProductCT
							{
								ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
								ProductName = reader.GetString(reader.GetOrdinal("product_name")),
								BrandId = reader.GetInt32(reader.GetOrdinal("brand_id")),
								BrandName = reader.GetString(reader.GetOrdinal("brand_name")),
								CategoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
								CategoryName = reader.GetString(reader.GetOrdinal("category_name")),
								Description = reader.GetString(reader.GetOrdinal("category_description")),
								Price = reader.GetDecimal(reader.GetOrdinal("price")),
								Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
								ImageUrl = reader.GetString(reader.GetOrdinal("image_url")),
							};
						}
					}
				}
			}

			if (productct == null)
			{
				return NotFound($"Product with ID {id} not found.");
			}

			return Ok(productct);
		}

		[HttpGet("Search")]
		public IActionResult SearchProducts([FromQuery] string? name, [FromQuery] string? brandName)
		{
			List<ProductCT> productcts = new List<ProductCT>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				// Truy vấn sản phẩm với các từ khóa liên quan đến máy tính/laptop
				string query = @"
			SELECT p.product_id, p.product_name, p.brand_id, p.category_id, p.price, p.quantity, p.image_url,
				b.brand_name, c.category_name, c.description AS category_description
			FROM Products p
			JOIN Brands b ON p.brand_id = b.brand_id
			JOIN Categories c ON p.category_id = c.category_id
			WHERE (@Name IS NULL OR p.product_name LIKE '%' + @Name + '%'
								OR p.product_name LIKE '%laptop%'
								OR p.product_name LIKE '%computer%'
								OR p.product_name LIKE '%pc%'
								OR p.product_name LIKE '%notebook%')
			AND (@BrandName IS NULL OR b.brand_name LIKE '%' + @BrandName + '%')";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					// Thêm tham số truy vấn
					command.Parameters.AddWithValue("@Name", string.IsNullOrEmpty(name) ? DBNull.Value : name);
					command.Parameters.AddWithValue("@BrandName", string.IsNullOrEmpty(brandName) ? DBNull.Value : brandName);

					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							var productct = new ProductCT
							{
								ProductId = reader.GetInt32(reader.GetOrdinal("product_id")),
								ProductName = reader.GetString(reader.GetOrdinal("product_name")),
								BrandId = reader.GetInt32(reader.GetOrdinal("brand_id")),
								BrandName = reader.GetString(reader.GetOrdinal("brand_name")),
								CategoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
								CategoryName = reader.GetString(reader.GetOrdinal("category_name")),
								Description = reader.GetString(reader.GetOrdinal("category_description")),
								Price = reader.GetDecimal(reader.GetOrdinal("price")),
								Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
								ImageUrl = reader.GetString(reader.GetOrdinal("image_url")),
							};

							productcts.Add(productct);
						}
					}
				}
			}

			// Xử lý trường hợp không tìm thấy sản phẩm
			if (productcts.Count == 0)
			{
				return NotFound(new { message = "Không tìm thấy sản phẩm nào phù hợp với từ khóa liên quan đến máy tính hoặc laptop." });
			}

			return Ok(new { status = "success", data = productcts });
		}
		
		
		// PUT: api/Products/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { message = "Dữ liệu không hợp lệ.", errors = ModelState });
			}

			using (var connection = new SqlConnection(_connectionString))
			{
				var sql = "UPDATE Products " +
						  "SET product_name = @ProductName, " +
						  "    category_id = @CategoryId, " +
						  "    price = @Price, " +
						  "    quantity = @Quantity, " +
						  "    image_url = @ImageUrl, " +
						  "    brand_id = @BrandId " +
						  "WHERE product_id = @ProductId";

				using (var command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@ProductId", id);
					command.Parameters.AddWithValue("@ProductName", product.ProductName);
					command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
					command.Parameters.AddWithValue("@Price", product.Price);
					command.Parameters.AddWithValue("@Quantity", product.Quantity);
					command.Parameters.AddWithValue("@ImageUrl", (object)product.ImageUrl ?? DBNull.Value);
					command.Parameters.AddWithValue("@BrandId", (object)product.BrandId ?? DBNull.Value);

					try
					{
						await connection.OpenAsync();
						int rowsAffected = await command.ExecuteNonQueryAsync();

						if (rowsAffected > 0)
						{
							return Ok(new { message = "Cập nhật sản phẩm thành công." });
						}
						else
						{
							return NotFound(new { message = "Không tìm thấy sản phẩm hoặc không có thay đổi." });
						}
					}
					catch (SqlException ex)
					{
						return StatusCode(500, new { message = $"Lỗi cơ sở dữ liệu: {ex.Message}" });
					}
				}
			}
		}

		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<IActionResult> CreateProduct([FromBody] Product product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { message = "Dữ liệu không hợp lệ.", errors = ModelState });
			}

			// Kiểm tra xem tên sản phẩm đã tồn tại trong cơ sở dữ liệu không
			using (var connection = new SqlConnection(_connectionString))
			{
				var checkProductSql = @"
			SELECT COUNT(*) FROM Products WHERE product_name = @ProductName";

				using (var checkCommand = new SqlCommand(checkProductSql, connection))
				{
					checkCommand.Parameters.AddWithValue("@ProductName", product.ProductName);

					try
					{
						await connection.OpenAsync();
						int existingProductCount = (int)await checkCommand.ExecuteScalarAsync();

						if (existingProductCount > 0)
						{
							return Conflict(new { message = "Tên sản phẩm đã tồn tại trong cơ sở dữ liệu." });
						}
					}
					catch (SqlException ex)
					{
						return StatusCode(500, new { message = $"Lỗi cơ sở dữ liệu khi kiểm tra sản phẩm: {ex.Message}" });
					}
				}

				// Nếu tên sản phẩm chưa tồn tại, thêm mới sản phẩm vào cơ sở dữ liệu
				var sql = @"
			INSERT INTO Products (product_name, category_id, price, quantity, image_url, brand_id)
			VALUES (@ProductName, @CategoryId, @Price, @Quantity, @ImageUrl, @BrandId)";

				using (var command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@ProductName", product.ProductName);
					command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
					command.Parameters.AddWithValue("@Price", product.Price);
					command.Parameters.AddWithValue("@Quantity", product.Quantity);
					command.Parameters.AddWithValue("@ImageUrl", product.ImageUrl ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@BrandId", product.BrandId);

					try
					{
						int rowsAffected = await command.ExecuteNonQueryAsync();

						if (rowsAffected > 0)
						{
							return Ok(new { message = "Sản phẩm được tạo thành công." });
						}
						else
						{
							return BadRequest(new { message = "Không thể tạo sản phẩm." });
						}
					}
					catch (SqlException ex)
					{
						return StatusCode(500, new { message = $"Lỗi cơ sở dữ liệu khi tạo sản phẩm: {ex.Message}" });
					}
				}
			}
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteProduct(int id)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					connection.Open();

					// Bắt đầu transaction để bảo đảm tính toàn vẹn dữ liệu
					using (SqlTransaction transaction = connection.BeginTransaction())
					{
						try
						{
							// Xóa các bản ghi trong bảng Reviews
							string deleteReviewsQuery = "DELETE FROM Reviews WHERE product_id = @Id";
							using (SqlCommand deleteReviewsCommand = new SqlCommand(deleteReviewsQuery, connection, transaction))
							{
								deleteReviewsCommand.Parameters.AddWithValue("@Id", id);
								deleteReviewsCommand.ExecuteNonQuery();
							}

							// Xóa các bản ghi trong bảng Order_Details
							string deleteOrderDetailsQuery = "DELETE FROM dbo.Order_Details WHERE product_id = @Id";
							using (SqlCommand deleteOrderDetailsCommand = new SqlCommand(deleteOrderDetailsQuery, connection, transaction))
							{
								deleteOrderDetailsCommand.Parameters.AddWithValue("@Id", id);
								deleteOrderDetailsCommand.ExecuteNonQuery();
							}

							// Xóa sản phẩm trong bảng Products
							string deleteProductQuery = "DELETE FROM Products WHERE product_id = @Id";
							using (SqlCommand deleteProductCommand = new SqlCommand(deleteProductQuery, connection, transaction))
							{
								deleteProductCommand.Parameters.AddWithValue("@Id", id);
								int rowsAffected = deleteProductCommand.ExecuteNonQuery();

								if (rowsAffected > 0)
								{
									// Commit transaction nếu không có lỗi
									transaction.Commit();
									return Ok(new { message = "Product and associated order details and reviews deleted successfully" });
								}
								else
								{
									// Nếu không tìm thấy sản phẩm, roll back transaction
									transaction.Rollback();
									return NotFound($"Product with ID {id} not found.");
								}
							}
						}
						catch (Exception ex)
						{
							// Rollback nếu có lỗi trong transaction
							transaction.Rollback();
							return StatusCode(500, $"Internal server error: {ex.Message}");
						}
					}
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		private bool ProductExists(int id)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				using (SqlCommand command = new SqlCommand("SELECT COUNT(1) FROM Products WHERE product_id = @id", connection))
				{
					// Thêm tham số vào câu lệnh SQL
					command.Parameters.AddWithValue("@id", id);

					// Thực thi câu lệnh và kiểm tra xem có kết quả trả về không
					int count = (int)command.ExecuteScalar(); // ExecuteScalar trả về giá trị đầu tiên của câu lệnh SELECT

					// Nếu COUNT trả về lớn hơn 0, nghĩa là sản phẩm tồn tại
					return count > 0;


				}
			}
		}
	}
}