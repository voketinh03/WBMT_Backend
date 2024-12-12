using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Ini;
using WBMT.Data;
using WBMT.Models;

namespace WBMT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHistoryModelsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrderHistoryModelsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/OrderHistoryModels
        [HttpGet("Lichsudonhang")]
        public JsonResult OrderHistory()
        {
         string query = @"
            SELECT 
            Orders.order_id, 
            Orders.customer_id, 
            Orders.create_at_order,
            Products.product_name, 
            Order_Details.number_of_products, 
            Orders.total_amount, 
            Orders.order_status
            FROM 
                Orders
            JOIN 
                Users ON Users.user_id = Orders.customer_id
            JOIN 
                Order_Details ON Order_Details.order_id = Orders.order_id
            JOIN 
                Products ON Products.product_id = Order_Details.product_id";

            string sqlDataSource = _configuration.GetConnectionString("WBMT");
            List<OrderHistoryModel> OHList = new List<OrderHistoryModel>();

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        using (SqlDataReader reader = myCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderHistoryModel listOrder = new OrderHistoryModel
                                {
                                    IdOrder = reader["order_id"]?.ToString(),
                                    Id = reader["customer_id"]?.ToString(),
                                    OrderDate = (DateTime)(reader["create_at_order"] != DBNull.Value ? Convert.ToDateTime(reader["create_at_order"]) : (DateTime?)null),
                                    ProductName = reader["product_name"]?.ToString(),
                                    Quantity = reader["number_of_products"].GetHashCode(),
                                    //!= DBNull.Value ? Convert.ToInt32(reader["number_of_products"]) : 0,
                                    Price = reader["total_amount"].GetHashCode(),
                                    //!= DBNull.Value ? Convert.ToDecimal(reader["total_amount"]) : 0m,
                                    OrderStatus = reader["order_status"]?.ToString()
                                };
                                OHList.Add(listOrder);
                            }
                        }
                    }
                }

                return new JsonResult(new { status = "success", data = OHList });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new JsonResult(new { status = "error", message = $"An error occurred: {ex.Message}" });
            }
        }
        //truy theo id đơn hàng
        [HttpGet("Lichsudonhang/{customerId}")]
        public JsonResult OrderHistory(int customerId)
        { 
       string query = @"
        SELECT 
            Orders.order_id, 
            Orders.customer_id, 
            Products.product_name, 
            Order_Details.number_of_products, 
            Orders.total_amount, 
            Orders.order_status
            FROM 
                Orders
            JOIN 
                Users ON Users.user_id = Orders.customer_id
            JOIN 
                Order_Details ON Order_Details.order_id = Orders.order_id
            JOIN 
                Products ON Products.product_id = Order_Details.product_id
            WHERE 
                Orders.customer_id = @CustomerId";  // Filter orders by customer ID

            string sqlDataSource = _configuration.GetConnectionString("WBMT");
            List<OrderHistoryModel> OHList = new List<OrderHistoryModel>();

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        // Add the customerId parameter to the SQL command
                        myCommand.Parameters.AddWithValue("@CustomerId", customerId);

                        using (SqlDataReader reader = myCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderHistoryModel listOrder = new OrderHistoryModel
                                {
                                    IdOrder = reader["order_id"]?.ToString(),
                                    Id = reader["customer_id"]?.ToString(),
                                    ProductName = reader["product_name"]?.ToString(),
                                    Quantity = reader["number_of_products"] != DBNull.Value ? Convert.ToInt32(reader["number_of_products"]) : 0,
                                    Price = reader["total_amount"] != DBNull.Value ? Convert.ToDecimal(reader["total_amount"]) : 0m,
                                    OrderStatus = reader["order_status"]?.ToString()
                                };
                                OHList.Add(listOrder);
                            }
                        }
                    }
                }

                return new JsonResult(new { status = "success", data = OHList });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new JsonResult(new { status = "error", message = $"An error occurred: {ex.Message}" });
            }
        }


    }
}

