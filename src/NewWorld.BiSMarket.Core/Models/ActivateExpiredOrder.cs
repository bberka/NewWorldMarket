namespace NewWorld.BiSMarket.Core.Models;

public class ActivateExpiredOrder
{
    public Guid UserGuid { get; set; }
    public Guid OrderGuid { get; set; }
}