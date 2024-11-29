using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WBMT.Models;
using WBMT.Model;

namespace WBMT.Data
{
    public class WBMTContext : DbContext
    {
        public WBMTContext (DbContextOptions<WBMTContext> options)
            : base(options)
        {
        }

        public DbSet<WBMT.Models.UserModel> UserModel { get; set; } = default!;
        public DbSet<WBMT.Models.ReUsersModel> reUsersModels { get; set; } = default!;
        public DbSet<WBMT.Models.CustomerModel> CustomerModel { get; set; } = default!;

        public DbSet<WBMT.Models.OrderHistoryModel> OrderHistoryModels { get; set; } = default!;
        public DbSet<WBMT.Model.Category> Category { get; set; } = default!;
        public DbSet<WBMT.Models.Product> Product { get; set; } = default!;
        public DbSet<WBMT.Models.Brand> Brand { get; set; } = default!;

    }
}
