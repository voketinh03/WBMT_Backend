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
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private static List<Brand> brands = new List<Brand>
        {
            new Brand { BrandId = 1, BrandName = "Dell" },
            new Brand { BrandId = 2, BrandName = "Apple" },
            new Brand { BrandId = 3, BrandName = "HP" },
            new Brand { BrandId = 4, BrandName = "Asus" }
        };

        [HttpGet]
        public IEnumerable<Brand> GetBrands()
        {
            return brands;
        }

        [HttpGet("{id}")]
        public Brand GetBrand(int id)
        {
            return brands.Find(b => b.BrandId == id);
        }

        [HttpPost]
        public IActionResult CreateBrand(Brand brand)
        {
            brands.Add(brand);
            return CreatedAtAction(nameof(GetBrand), new { id = brand.BrandId }, brand);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBrand(int id, Brand brand)
        {
            var existingBrand = brands.Find(b => b.BrandId == id);
            if (existingBrand == null)
                return NotFound();

            existingBrand.BrandName = brand.BrandName;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBrand(int id)
        {
            var brand = brands.Find(b => b.BrandId == id);
            if (brand == null)
                return NotFound();

            brands.Remove(brand);
            return NoContent();
        }


        /*private bool BrandExists(int id)
		{
			return _context.Brands.Any(e => e.BrandId == id);
		}*/
    }
}