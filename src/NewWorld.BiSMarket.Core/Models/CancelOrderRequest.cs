namespace NewWorld.BiSMarket.Core.Models;

public class CancelOrderRequest
{
    public Guid OrderGuid { get; set; }
    public Guid UserGuid { get; set; }
}