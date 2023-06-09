using NewWorldMarket.Entities;

namespace NewWorldMarket.Core.Models;

public class ActiveOrderData
{
    public List<Order> SellOrderList { get; set; } = new();
    public List<Order> BuyOrderList { get; set; } = new();
}