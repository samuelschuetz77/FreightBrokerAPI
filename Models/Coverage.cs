namespace FreightBrokerAPI.Models;

public class Coverage
{
    public int CoverageId { get; set; }         // PK
    public int LoadId { get; set; }             // FK → Load
    public Load? Load { get; set; }

    public int CarrierId { get; set; }          // FK → Carrier
    public Carrier? Carrier { get; set; }

    public decimal CarrierRate { get; set; }
    public DateTime CoveredAtUtc { get; set; } = DateTime.UtcNow;
}