using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBMT.Data;
using WBMT.Model;
using WBMT.Models;

namespace WBMT.Controllers
{
   private readonly string _connectionString;

    public BrandsController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("WBMT");
    }

    // GET: api/Brands
    [HttpGet]
    public async Task<IActionResult> GetBrands()
    {
        List<Brand> brands = new List<Brand>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Truy vấn lấy tất cả các thương hiệu
            string query = "SELECT brand_id, brand_name FROM Brands";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        brands.Add(new Brand
                        {
                            BrandId = reader.GetInt32(reader.GetOrdinal("brand_id")),
                            BrandName = reader.GetString(reader.GetOrdinal("brand_name"))
                        });
                    }
                }
            }
        }

        if (brands.Count == 0)
        {
            return NotFound("Không tìm thấy thương hiệu.");
        }

        return Ok(new { status = "success", data = brands });
    }

    // GET: api/Brands/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrand(int id)
    {
        Brand brand = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Truy vấn tìm thương hiệu theo ID
            string query = "SELECT brand_id, brand_name FROM Brands WHERE brand_id = @BrandId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BrandId", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        brand = new Brand
                        {
                            BrandId = reader.GetInt32(reader.GetOrdinal("brand_id")),
                            BrandName = reader.GetString(reader.GetOrdinal("brand_name"))
                        };
                    }
                }
            }
        }

        if (brand == null)
        {
            return NotFound($"Không tìm thấy thương hiệu với ID {id}.");
        }

        return Ok(brand);
    }

    // POST: api/Brands
    [HttpPost]
    public async Task<IActionResult> CreateBrand([FromBody] Brand brand)
    {
        if (brand == null || string.IsNullOrEmpty(brand.BrandName))
        {
            return BadRequest("Dữ liệu không hợp lệ.");
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Truy vấn tạo mới thương hiệu
            string query = "INSERT INTO Brands (brand_name) VALUES (@BrandName)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BrandName", brand.BrandName);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    // Trả về kết quả sau khi tạo thương hiệu thành công
                    return CreatedAtAction(nameof(GetBrand), new { id = brand.BrandId }, brand);
                }
                else
                {
                    return StatusCode(500, "Không thể tạo thương hiệu.");
                }
            }
        }
    }

    // PUT: api/Brands/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBrand(int id, [FromBody] Brand brand)
    {
        if (brand == null || string.IsNullOrEmpty(brand.BrandName))
        {
            return BadRequest("Dữ liệu không hợp lệ.");
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Truy vấn cập nhật thương hiệu theo ID
            string query = "UPDATE Brands SET brand_name = @BrandName WHERE brand_id = @BrandId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BrandId", id);
                command.Parameters.AddWithValue("@BrandName", brand.BrandName);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    // Trả về khi cập nhật thành công
                    return NoContent();
                }
                else
                {
                    return NotFound($"Không tìm thấy thương hiệu với ID {id}.");
                }
            }
        }
    }

    // DELETE: api/Brands/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrand(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Truy vấn xóa thương hiệu theo ID
            string query = "DELETE FROM Brands WHERE brand_id = @BrandId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BrandId", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    // Trả về khi xóa thành công
                    return NoContent();
                }
                else
                {
                    return NotFound($"Không tìm thấy thương hiệu với ID {id}.");
                }
            }
        }
    }
}