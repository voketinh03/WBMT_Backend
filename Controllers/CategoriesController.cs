using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using WBMT.Model;
using System.IO;
using Microsoft.AspNetCore.Hosting;

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

        [HttpDelete]
        public JsonResult Delete(int ma)
        {
            string query = @"Delete From Categories Where category_id = " + ma;
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
            return new JsonResult("Xoa bo thanh cong");
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