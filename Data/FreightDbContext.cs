using Microsoft.EntityFrameworkCore;
using FreightBrokerAPI.Models;

namespace FreightBrokerAPI.Data;

public class FreightDbContext : DbContext
{
    public FreightDbContext(DbContextOptions<FreightDbContext> options) : base(options) { }

    public DbSet<Shipper> Shippers => Set<Shipper>();
    public DbSet<Carrier> Carriers => Set<Carrier>();
    public DbSet<Load> Loads => Set<Load>();
    public DbSet<Coverage> Coverages => Set<Coverage>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Coverage>().HasIndex(c => c.LoadId).IsUnique();
        b.Entity<Load>().HasQueryFilter(l => !l.IsDeleted);
    }
}
