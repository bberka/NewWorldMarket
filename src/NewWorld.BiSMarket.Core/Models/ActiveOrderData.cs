using NewWorld.BiSMarket.Core.Entity;

namespace NewWorld.BiSMarket.Core.Models;

public class ActiveOrderData
{
    public List<Order> SellOrderList { get; set; } = new();
    public List<Order> BuyOrderList { get; set; } = new();
}