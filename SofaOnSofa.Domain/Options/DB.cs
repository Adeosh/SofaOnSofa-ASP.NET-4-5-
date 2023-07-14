using SofaOnSofa.Domain.Entities;
using System.Data.Entity;

namespace SofaOnSofa.Domain.Options
{
    public class DB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
