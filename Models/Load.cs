namespace FreightBrokerAPI.Models;


public enum LoadStatus { Open = 0, Covered = 1, InTransit = 2, Delivered = 3, Canceled = 4 }

public class Load
{
    public int LoadId { get; set; }             // PK
    public int ShipperId { get; set; }          // FK → Shipper
    public Shipper? Shipper { get; set; }

    public string Origin { get; set; } = "";
    public string Destination { get; set; } = "";
    public decimal ShipperRate { get; set; }
    public LoadStatus Status { get; set; } = LoadStatus.Open;
    public bool IsDeleted { get; set; } = false;

    public Coverage? Coverage { get; set; }     // each load can have at most one coverage
}