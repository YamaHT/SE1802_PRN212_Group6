using Microsoft.EntityFrameworkCore;
using SE1802_PRN212_Group6.Models.Enums;
using SE1802_PRN212_Group6.Models;

namespace SE1802_PRN212_Group6.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Table> Table { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Voucher> Voucher { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=PRN212_SE1802_Group6;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                Enum.GetValues(typeof(CategoryEnum))
                    .Cast<CategoryEnum>()
                    .Select(categoryEnum => new Category
                    {
                        Id = (int)categoryEnum,
                        Name = categoryEnum.ToString()
                    })
            );
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<double>().HaveColumnType("numeric(10, 2)");
        }
    }
}
