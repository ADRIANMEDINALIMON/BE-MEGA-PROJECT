using BE_MEGA_PROJECT.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_MEGA_PROJECT.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options) {
        
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageService> PackageServices { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<ContractPromotion> ContractPromotions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoicePromotion> InvoicePromotions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContractPromotion>()
                .HasKey(cp => new { cp.ContractId, cp.PromotionId });

            modelBuilder.Entity<InvoicePromotion>()
                .HasKey(ip => new { ip.InvoiceId, ip.PromotionId });


            modelBuilder.Entity<PackageService>()
                .HasKey(ps => new { ps.PackageId, ps.ServiceId });
            modelBuilder.Entity<Invoice>()
    .Property(i => i.BaseAmount)
    .HasPrecision(10, 2);
            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasPrecision(10, 2);
            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalDiscount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Service>()
                .Property(s => s.MonthlyPrice)
                .HasPrecision(10, 2);
            modelBuilder.Entity<Service>()
                .Property(s => s.SetupPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<InvoicePromotion>()
                .Property(ip => ip.DiscountAmount)
                .HasPrecision(10, 2);

        }



    }
}
