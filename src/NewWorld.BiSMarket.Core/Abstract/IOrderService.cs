using EasMe.Result;
using Microsoft.AspNetCore.Http;
using NewWorld.BiSMarket.Core.Entity;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Core.Abstract;

public interface IOrderService
{
    ResultData<List<Order>> GetMainPageSellOrders(int region = -1, int server = -1,int page = 1);
    ResultData<List<Order>> GetMainPageBuyOrders(int region = -1, int server = -1,int page = 1);
    ResultData<List<Order>> GetOrdersByUsername(byte type, string username, int page);
    ResultData<List<Order>> GetOrdersByUserGuid(byte type, Guid userGuid);

    ResultData<Order> GetOrderById(Guid orderGuid);
    ResultData<List<Order>> GetOrderByHash(string hash);
    ResultData<List<Order>> GetOrderByHash(Guid userGuid,string hash);


    ResultData<Item> UploadItemImageAndGetItemData(IFormFile file);

    //order
    Result CreateOrder(CreateOrder request);
    Result CancelOrder(CancelOrder request);
    Result ActivateExpiredOrder(ActivateExpiredOrder request);
    Result CreateOrderRequest(CreateOrderRequest request);
    Result CancelOrderRequest(CancelOrderRequest request);

}