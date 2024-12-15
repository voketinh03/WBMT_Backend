using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using WBMT.Model;
using System.IO;
using Microsoft.AspNetCore.Hosting;


using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WBMT.Models;


namespace WBMT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class  CategoriesController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public CategoriesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = "Select category_id, category_name, description from Categories";
            DataTable table = new DataTable();
            String SqlDataSource = _configuration.GetConnectionString("WBMT");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(SqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        //[Route("SaveFile")]
        [HttpPost]
        public JsonResult Post(Category danhmuc)
        {
            //'" + danhmuc.CategoryId + "' " +
            //       ", 
            string query = @"Insert into Categories values 
                   ('" + danhmuc.CategoryName + "'" +
                   ",'" + danhmuc.Description + "' " + " )";

            //"INSERT INTO Categories(category_id, category_name, description) VALUES('"gia_t+'")";
            DataTable table = new DataTable();
            String SqlDataSource = _configuration.GetConnectionString("WBMT");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(SqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Them moi thanh cong");
        }

        [HttpPut]
        public JsonResult Put(Category danhmuc, int ma)
        {
            string query = @"UPDATE Categories SET
                  category_name = '" + danhmuc.CategoryName + "'" +
                 ", description='" + danhmuc.Description + "'" +
                 "  WHERE category_id= '" + ma + "'";

            DataTable table = new DataTable();
            String SqlDataSource = _configuration.GetConnectionString("WBMT");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(SqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Cap nhat thanh cong");
        }

        [HttpDelete("{id}")]
        
        public JsonResult Delete(int id)
        {
            try
            {
                // Query để xóa dữ liệu liên quan trong bảng Order_Details
                
                // Query để xóa dữ liệu liên quan trong bảng Orders
               

                // Query để xóa bản ghi trong bảng Users nếu role_id = 2 và name = 'user'
                string deleteCategoriesQuery = @"
            DELETE FROM Categories
            WHERE category_id = @Id";


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
                            

                            // Xóa dữ liệu trong bảng Orders
                            

                            // Xóa dữ liệu trong bảng Users
                            using (SqlCommand userCommand = new SqlCommand(deleteCategoriesQuery, myCon, transaction))
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

        [Route("GetAllDanhmuc")]
        [HttpGet]
        public JsonResult GetAllDanhmuc()
        {
            string query = "select category_name from Categories";
            DataTable table = new DataTable();
            String SqlDataSource = _configuration.GetConnectionString("WBMT");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(SqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }


    }
}