using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using WBMT.Data;
using WBMT.Models;

namespace WBMT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserModelsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
            
        public UserModelsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("dangnhap")]
        public JsonResult DN([FromBody] UserModel userModel)
        {
            try
            {
                
                string query = @"
            SELECT u.username, u.password, r.id AS role_id, r.name
            FROM Users u
            JOIN Roles r ON u.role_id = r.id
            WHERE u.username = @username AND u.password = @password";

                string sqlDataSource = _configuration.GetConnectionString("WBMT");
                string role = string.Empty;
                string username = string.Empty;
                string roleName = string.Empty;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@username", userModel.UserName);
                        myCommand.Parameters.AddWithValue("@password", userModel.Password);

                        using (SqlDataReader reader = myCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                username = reader["username"].ToString();
                                roleName = reader["name"].ToString();
                            }
                        }
                    }
                }

                // Kiểm tra xem người dùng có hợp lệ và có quyền gì
                if (!string.IsNullOrEmpty(username))
                {
                    // Điều hướng dựa trên role
                    string redirectUrl = roleName.ToLower() == "admin" ? "admin" : "user";

                    return new JsonResult(new
                    {
                        status = "success",
                        message = "Login successful",
                        data = new { username, roleName, redirectUrl }
                    });
                }
                else
                {
                    return new JsonResult(new { status = "failure", message = "Invalid username or password" });
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
