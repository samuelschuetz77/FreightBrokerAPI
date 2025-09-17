namespace FreightBrokerAPI.Models;

public class Shipper
{
    public int ShipperId { get; set; }          // PK
    public string Name { get; set; } = "";
    public ICollection<Load> Loads { get; set; } = new List<Load>();
}