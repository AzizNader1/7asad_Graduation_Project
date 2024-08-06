using Graduation_Web_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Web_App.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyAccount> CompanyAccounts { get; set; }
        public DbSet<Engineer> Engineers { get; set; }
        public DbSet<EngineerAccount> EngineerAccounts { get; set; }
        public DbSet<EngineerFarmer> EngineerFarmers { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<FarmerAccount> FarmerAccounts { get; set; }
        public DbSet<Land> Lands { get; set; }
        public DbSet<LandOrder> LandOrders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<Represintor> Represintors { get; set; }
        public DbSet<EngineerCompany> EngineerCompanies { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<FarmerEquipment> FarmerEquipments { get; set; }
        public DbSet<LogingUser> LogingUsers { get; set; }
        public DbSet<FileInformation> FileInformations { get; set; }
        public DbSet<BuyerFarmer> BuyerFarmers { get; set; }
        public DbSet<FarmerLandOrder> FarmerLandOrders { get; set; }
        public DbSet<FarmerProductOrder> FarmerProductOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure cascade delete for all relationships
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }
        }

    }
}
