using FreightBrokerAPI.Models;

namespace FreightBrokerAPI.Data;

public static class DbInitializer
{
    public static void Seed(FreightDbContext context)
    {
        if (context.Shippers.Any()) return; // DB already seeded

        var shippers = new Shipper[]
        {
            new Shipper { Name = "Acme Farms" },
            new Shipper { Name = "Global Warehousing" }
        };
        context.Shippers.AddRange(shippers);
        context.SaveChanges();

        var loads = new Load[]
        {
            new Load { ShipperId = shippers[0].ShipperId, Origin = "Iowa", Destination = "Chicago", ShipperRate = 1200m },
            new Load { ShipperId = shippers[1].ShipperId, Origin = "Denver", Destination = "Dallas", ShipperRate = 1800m }
        };
        context.Loads.AddRange(loads);
        context.SaveChanges();
    }
}
