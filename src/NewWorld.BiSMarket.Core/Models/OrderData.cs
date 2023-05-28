using NewWorld.BiSMarket.Core.Entity;

namespace NewWorld.BiSMarket.Core.Models;

public class OrderData
{
    public List<Order> SellOrderList { get; set; }
    public List<Order> BuyOrderList { get; set; }
}