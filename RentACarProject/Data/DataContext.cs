using Microsoft.EntityFrameworkCore;
using RentACarProject.Models;

namespace RentACarProject.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options ):base(options)
        {
            
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           
            modelBuilder.Entity<Car>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Customer>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Rent>()
          .Property(r => r.Id)
          .ValueGeneratedOnAdd();

            modelBuilder.Entity<Rent>()
                .HasKey(r => new { r.CustomerId, r.CarId });
        }
    }
}
