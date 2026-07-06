using ApiCriptomonedas.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiCriptomonedas.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Transacciones> Transacciones { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
    }
}
