namespace NewWorld.BiSMarket.Core.Models;

public class CreateOrderRequest
{
    public Guid OrderGuid { get; set; }
    public Guid UserGuid { get; set; }
    public Guid CharacterGuid { get; set; }
}