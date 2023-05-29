using NewWorld.BiSMarket.Core.Entity;

namespace NewWorld.BiSMarket.Core.Models;

public class CreateBuyOrder
{
    public List<Character> CharactersListForView { get; set; } = new();
    public Guid UserGuid { get; set; }
    public Guid CharacterGuid { get; set; }
    public Guid ImageGuid { get; set; }
    public float Price { get; set; }
    public int EstimatedDeliveryTimeHour { get; set; }
    /// <summary>
    /// 0: Buy, 1: Sell
    /// </summary>
    public byte Type { get; set; }
    public Item ItemData { get; set; }
}