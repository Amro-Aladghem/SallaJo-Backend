using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public DbSet<ActivationCode> ActivationCodes { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Governorate> Governorates { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<SellerRole> SellerRoles { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<UserType> UserTypes { get; set; }
    public DbSet<Seller> Sellers { get; set; }
}
