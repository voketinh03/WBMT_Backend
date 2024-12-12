using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WBMT.Data;
using WBMT.Models;

namespace WBMT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerModelsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CustomerModelsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/CustomerModels
        // GET: api/CustomerModels
        [HttpGet("GetThongTin")]
        public JsonResult ViewCustomer()
        {
            try
            {
                string query = @"
            SELECT user_id, username, email, phone, address
            FROM Users";

                string sqlDataSource = _configuration.GetConnectionString("WBMT");
                List<CustomerModel> customList = new List<CustomerModel>();

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        using (SqlDataReader reader = myCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerModel user = new CustomerModel
                                {
                                    Id = reader["user_id"].GetHashCode(),
                                    UserName = reader["username"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Phone = reader["phone"].ToString(),
                                    Address = reader["address"].ToString()
                                };
                                customList.Add(user);
                            }
                        }
                    }
                }

                return new JsonResult(new { status = "success", data = customList });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new JsonResult(new { status = "error", message = $"An error occurred: {ex.Message}" });
            }
        }
        //Dừng
       
        //xem theo id

        [HttpGet("GetTTCTid/{id}")]
        public JsonResult ViewCTCustomerid(int id)
        {
            try
            {
                string query = @"
        SELECT user_id, username, email, phone, password, address
        FROM Users
        WHERE user_id = @Id";  // Thêm điều kiện WHERE để lọc theo ID

                string sqlDataSource = _configuration.GetConnectionString("WBMT");
                List<CustomerCTModel> customCTList = new List<CustomerCTModel>();

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Id", id); // Thêm tham số ID vào truy vấn

                        using (SqlDataReader reader = myCommand.ExecuteReader())
                        {
                            if (reader.Read()) // Chỉ cần đọc một dòng vì chúng ta lọc theo ID
                            {
                                CustomerCTModel user = new CustomerCTModel
                                {
                                    Id = (int)reader["user_id"],
                                    UserName = reader["username"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Phone = reader["phone"].ToString(),
                                    Address = reader["address"].ToString(),
                                    Password = reader["password"].ToString()
                                };
                                customCTList.Add(user);
                            }
                        }
                    }
                }

                return new JsonResult(new { status = "success", data = customCTList });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new JsonResult(new { status = "error", message = $"An error occurred: {ex.Message}" });
            }
        }
        // PUT: api/CustomerModels/UpdateCustomer
        [HttpPut("UpdateCustome/{id}")]
        public JsonResult UpdateCustomer(int id, [FromBody] CustomerCTModel updatedCustomer)
        {
            try
            {
                // Ensure that the provided ID from the route is used, not the one from the request body
                if (id != updatedCustomer.Id)
                {
                    return new JsonResult(new { status = "error", message = "The ID in the URL and the ID in the body do not match" });
                }

                string query = @"
            UPDATE Users
            SET username = @UserName, email = @Email, phone = @Phone, address = @Address, password = @Password
            WHERE user_id = @Id";  // Update query with parameters

                string sqlDataSource = _configuration.GetConnectionString("WBMT");

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        // Add parameters to the query
                        myCommand.Parameters.AddWithValue("@Id", id); // Using the ID from the route, not the body
                        myCommand.Parameters.AddWithValue("@UserName", updatedCustomer.UserName);
                        myCommand.Parameters.AddWithValue("@Email", updatedCustomer.Email);
                        myCommand.Parameters.AddWithValue("@Phone", updatedCustomer.Phone);
                        myCommand.Parameters.AddWithValue("@Address", updatedCustomer.Address);
                        myCommand.Parameters.AddWithValue("@Password", updatedCustomer.Password);

                        int rowsAffected = myCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return new JsonResult(new { status = "success", message = "Customer updated successfully" });
                        }
                        else
                        {
                            return new JsonResult(new { status = "error", message = "Customer not found or no changes made" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new JsonResult(new { status = "error", message = $"An error occurred: {ex.Message}" });
            }
        } 
       //xóa theo id
        [HttpDelete("DeleteCustomer/{id}")]
        public JsonResult DeleteCustomer(int id)
        {
            try  
            {
                // Query để xóa dữ liệu liên quan trong bảng Order_Details
           string deleteOrderDetailsQuery = @"
            DELETE FROM Order_Details
            WHERE order_id IN (SELECT order_id FROM Orders WHERE customer_id = @Id)";
                // Query để xóa dữ liệu liên quan trong bảng Orders
           string deleteOrdersQuery = @"
            DELETE FROM Orders
            WHERE customer_id = @Id";

                // Query để xóa bản ghi trong bảng Users nếu role_id = 2 và name = 'user'
           string deleteUserQuery = @"
            DELETE FROM Users
            WHERE user_id = @Id 
              AND role_id = (SELECT id FROM Roles WHERE name = 'user')";


                string sqlDataSource = _configuration.GetConnectionString("WBMT");

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();

                    // Sử dụng transaction để đảm bảo tính toàn vẹn dữ liệu
                    using (SqlTransaction transaction = myCon.BeginTransaction())
                    {
                        try
                        {
                            // Xóa dữ liệu trong bảng Order_Details
                            using (SqlCommand orderDetailsCommand = new SqlCommand(deleteOrderDetailsQuery, myCon, transaction))
                            {
                                orderDetailsCommand.Parameters.AddWithValue("@Id", id);
                                orderDetailsCommand.ExecuteNonQuery();
                            }

                            // Xóa dữ liệu trong bảng Orders
                            using (SqlCommand ordersCommand = new SqlCommand(deleteOrdersQuery, myCon, transaction))
                            {
                                ordersCommand.Parameters.AddWithValue("@Id", id);
                                ordersCommand.ExecuteNonQuery();
                            }

                            // Xóa dữ liệu trong bảng Users
                            using (SqlCommand userCommand = new SqlCommand(deleteUserQuery, myCon, transaction))
                            {
                                userCommand.Parameters.AddWithValue("@Id", id);
                                int rowsAffected = userCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    transaction.Commit();
                                    return new JsonResult(new { status = "success", message = "Customer deleted successfully" });
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return new JsonResult(new { status = "error", message = "Customer not found" });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Error during transaction: {ex.Message}");
                            return new JsonResult(new { status = "error", message = $"An error occurred: {ex.Message}" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new JsonResult(new { status = "error", message = $"An error occurred: {ex.Message}" });
            }
        }



    }
}