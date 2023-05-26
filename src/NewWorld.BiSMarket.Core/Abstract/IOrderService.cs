using EasMe.Result;
using Microsoft.AspNetCore.Http;
using NewWorld.BiSMarket.Core.Entity;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Core.Abstract;

public interface IOrderService
{
    ResultData<List<Order>> GetOrdersMainPage(byte type,int region, int server,int page);
    ResultData<List<Order>> GetOrdersByUsername(byte type, string username, int page);
    ResultData<List<Order>> GetOrdersByUserGuid(byte type, Guid userGuid, int page);

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