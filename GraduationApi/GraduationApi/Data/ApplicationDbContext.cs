using GraduationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyAccount> CompanyAccounts { get; set; }
        public DbSet<EngineerCompany> EngineerCompanies { get; set; }
        public DbSet<Engineer> Engineers { get; set; }
        public DbSet<EngineerAccount> EngineerAccounts { get; set; }
        public DbSet<EngineerFarmer> EngineerFarmers { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<FarmerAccount> FarmerAccounts { get; set; }
        public DbSet<FarmerEquipment> FarmerEquipments { get; set; }
        public DbSet<Land> Lands { get; set; }
        public DbSet<LandOrder> LandOrders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<Represintor> Represintors { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<LogingUser> LogingUsers { get; set; }
        public DbSet<FileInformation> FileInformations { get; set; }
        public DbSet<FarmerLandOrder> FarmerLandOrders { get; set; }
        public DbSet<FarmerProductOrder> FarmerProductOrders { get; set; }
        public DbSet<BuyerFarmer> BuyerFarmers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure specific delete behaviors

            // Configure FarmerEquipment relationship
            modelBuilder.Entity<FarmerEquipment>()
                .HasOne(fe => fe.Farmer)
                .WithMany(f => f.FarmerEquipments)
                .HasForeignKey(fe => fe.FarmerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            modelBuilder.Entity<FarmerEquipment>()
                .HasOne(fe => fe.Equipment)
                .WithMany(f => f.FarmerEquipments)
                .HasForeignKey(fe => fe.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // You may have other configurations for different relationships as needed

            // Remove the default cascade delete behavior
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }
    }
}
