using DeadStockHair.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DeadStockHair.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Retailer> Retailers => Set<Retailer>();
    public DbSet<SavedRetailer> SavedRetailers => Set<SavedRetailer>();
    public DbSet<ScanResult> ScanResults => Set<ScanResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Retailer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.DiscoveredAt).IsRequired();
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Url);
        });

        modelBuilder.Entity<SavedRetailer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RetailerId).IsRequired();
            entity.Property(e => e.SavedAt).IsRequired();
            entity.HasIndex(e => e.RetailerId);
        });

        modelBuilder.Entity<ScanResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.StartedAt).IsRequired();
        });
    }
}
