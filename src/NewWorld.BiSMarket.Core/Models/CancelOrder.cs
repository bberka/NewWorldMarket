namespace NewWorld.BiSMarket.Core.Models;

public class CancelOrder
{
    public Guid UserGuid { get; set; }
    public Guid OrderGuid { get; set; }
}