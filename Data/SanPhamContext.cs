using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data
{
    public class SanPhamContext : DbContext
    {
        public SanPhamContext(DbContextOptions<SanPhamContext> options) : base (options) { }

        public DbSet<SanPham> SanPhams { get; set; }
    }
}
