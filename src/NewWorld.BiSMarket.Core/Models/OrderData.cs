using NewWorld.BiSMarket.Core.Entity;

namespace NewWorld.BiSMarket.Core.Models;

public class OrderData
{
    public List<Order> ActiveSellOrderList { get; set; }
    public List<Order> ActiveBuyOrderList { get; set; }
    public List<Order> ExpiredOrderList { get; set; }
    public List<Order> CancelledOrderList { get; set; }
    public List<Order> CompletedOrderList { get; set; }
}