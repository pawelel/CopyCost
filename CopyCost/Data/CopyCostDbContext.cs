using CopyCost.Entities;
using Microsoft.EntityFrameworkCore;

namespace CopyCost.Data;

public sealed class CopyCostDbContext : DbContext
{
    public CopyCostDbContext(DbContextOptions<CopyCostDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>()
            .HasKey(d => d.Id);
        modelBuilder.Entity<Category>()
            .HasKey(ur => new { ur.Id });
        modelBuilder.Entity<Customer>()
            .HasKey(ur => new { ur.Id });
        modelBuilder.Entity<Payment>()
            .HasOne(ur => ur.Category)
            .WithMany(u => u.Payments)
            .HasForeignKey(ur => ur.CategoryId);
        modelBuilder.Entity<Payment>()
            .HasOne(ur => ur.Customer)
            .WithMany(r => r.Payments)
            .HasForeignKey(ur => ur.CustomerId);
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Payments)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Payments)
            .WithOne(p => p.Customer)
            .HasForeignKey(p => p.CustomerId);
    }
}