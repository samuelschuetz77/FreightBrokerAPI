namespace FreightBrokerAPI.Models;

public class Carrier
{
    public int CarrierId { get; set; }          // PK
    public string Name { get; set; } = "";
    public string MCNumber { get; set; } = "";  // Motor Carrier # for real freight world
    public ICollection<Coverage> Coverages { get; set; } = new List<Coverage>();
}