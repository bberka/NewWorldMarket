using EasMe.Result;
using NewWorldMarket.Core.Entity;
using NewWorldMarket.Core.Models;

namespace NewWorldMarket.Core.Abstract;

public interface IOrderService
{
    ResultData<List<Order>> GetMainPageSellOrders(int region = -1, int server = -1, int page = 1);
    ResultData<List<Order>> GetMainPageBuyOrders(int region = -1, int server = -1, int page = 1);

    ResultData<List<Order>> GetOrdersByUsername(byte type, string username, int page);
    //ResultData<List<Order>> GetOrdersByUserGuid(byte type, Guid userGuid);

    ResultData<ActiveOrderData> GetUserOrders(Guid userGuid);
    ResultData<OrderData> GetUserOrderData(Guid userGuid);

    ResultData<Order> GetOrderById(Guid orderGuid);
    ResultData<List<Order>> GetOrderByHash(string hash);
    ResultData<List<Order>> GetOrderByHash(Guid userGuid, string hash);

    ResultData<List<Order>> GetFilteredActiveOrders(
        int attr = -1,
        int perk1 = -1,
        int perk2 = -1,
        int perk3 = -1,
        int type = -1,
        int server = -1,
        int rarity = -1
    );


    //order
    Result CreateOrder(CreateOrder request);
    Result CreateSellOrder(CreateSellOrder request);
    Result CreateBuyOrder(CreateBuyOrder request);
    Result CancelOrder(Guid userGuid, Guid orderGuid);
    Result CompleteOrder(Guid userGuid, Guid orderGuid);
    Result UpdateOrderPrice(Guid userGuid, Guid orderGuid, float price);

    Result ActivateExpiredOrder(Guid userGuid, Guid orderRequestGuid);
    Result CreateOrderRequest(CreateOrderRequest request);
    Result CancelOrderRequest(Guid userGuid, Guid orderRequestGuid);
}