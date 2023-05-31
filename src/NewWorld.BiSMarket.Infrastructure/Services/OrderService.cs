using Azure.Core;
using EasMe.Result;
using Microsoft.AspNetCore.Http;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Core.Constants;
using NewWorld.BiSMarket.Core.Entity;
using NewWorld.BiSMarket.Core.Models;
using System.Net.Http.Headers;
using EasMe.Extensions;
using NewWorld.BiSMarket.Core;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Image = NewWorld.BiSMarket.Core.Entity.Image;
using Ninject.Activation;
using shortid;
using shortid.Configuration;

namespace NewWorld.BiSMarket.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Result CreateSellOrder(CreateSellOrder request)
    {
        if (request.EstimatedDeliveryTimeHour > ConstMgr.MaxDeliveryTime)
        {
            return Result.Warn($"Estimated delivery time cannot be more than {ConstMgr.MaxDeliveryTime} hours.");
        }
        if (request.EstimatedDeliveryTimeHour < 1)
        {
            return Result.Warn($"Estimated delivery time cannot be less than 1 hours.");
        }
        if (request.Price < 1000)
        {
            return Result.Warn($"Price cannot be less than 1000 coins.");
        }
        if (request.Price > ConstMgr.MaxPriceLimit)
        {
            return Result.Warn($"Price cannot be more than {ConstMgr.MaxPriceLimit} coins.");
        }

        //This hash check can be improved and its not tested so it may not be working correctly
        var exists = _unitOfWork.OrderRepository.Any(x => x.CharacterGuid == request.CharacterGuid 
                                                          && x.Hash == request.UniqueHash 
                                                          && !x.CancelledDate.HasValue
                                                          && !x.CompletedDate.HasValue
                                                          && x.ExpirationDate > DateTime.Now);
        if (exists)
            return Result.Warn("It looks like you already listed the same item, if you think this is a mistake contact us.");
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == request.UserGuid);
        if (user == null)
            return Result.Warn("User not found.");
        var characterList = _unitOfWork.CharacterRepository.Get(x => x.UserGuid == request.UserGuid).Select(x => new 
        {
            Guid = x.Guid,
            Region = x.Region,
            Server = x.Server,
            Name = x.Name,
        });
        var character = characterList.FirstOrDefault(x => x.Guid == request.CharacterGuid);
        if (character == null)
            return Result.Warn("Character not found.");
        var charGuidList = characterList.Select(x => x.Guid).ToList();
        var currentOrders = _unitOfWork.OrderRepository
            .Count(x => charGuidList.Contains(x.CharacterGuid) 
                        && !x.CancelledDate.HasValue 
                        && !x.CompletedDate.HasValue 
                        && x.ExpirationDate > DateTime.Now);
        if (currentOrders >= ConstMgr.DefaultOrderCountLimit)
            return Result.Warn($"You can only list {currentOrders} items at a time.");
        var image = _unitOfWork.ImageRepository.GetById(request.ImageGuid);
        if (image == null)
            return Result.Warn("Image not found.");
        var itemData = image.OcrItemDataResult.FromJsonString<Item>();
        if (itemData == null)
            return Result.Warn("Item data not found.");
        if(itemData.ItemType == -1) itemData.ItemType = request.ItemType;
        if(itemData.LevelRequirement == -1) itemData.LevelRequirement = request.LevelRequirement;
        if(itemData.Tier == -1) itemData.Tier = request.Tier;
        if(itemData.Rarity == -1) itemData.Rarity = request.Rarity;
        if(itemData.Perks == string.Empty) itemData.Perks = request.Perks;
        if(itemData.Attributes == string.Empty) itemData.Attributes = request.Attributes;
        if(itemData.GemId == -1) itemData.GemId = request.GemId;
        itemData.IsGemChangeable = request.IsGemChangeable;
        if(itemData.IsNamed == null) itemData.IsNamed = request.IsNamed;
        if(itemData.GearScore == -1) itemData.GearScore = request.GearScore;
        




        var oq = new Order
        {
            Guid = Guid.NewGuid(),
            CharacterGuid = request.CharacterGuid,
            Type = request.Type,
            Region = character.Region,
            Server = character.Server,
            Hash = itemData.UniqueHash,
            Price = request.Price,
            RegisterDate = DateTime.Now,
            IsNamed = itemData.IsNamed ?? false,
            GemId = itemData.GemId,
            Attributes = itemData.Attributes,
            CancelledDate = null,
            CompletedDate = null,
            ImageGuid = request.ImageGuid,
            EstimatedDeliveryTimeHour = request.EstimatedDeliveryTimeHour,
            ExpirationDate = DateTime.Now.AddDays(14),
            GearScore = itemData.GearScore,
            IsGemChangeable = itemData.IsGemChangeable,
            IsValid = true,
            ItemType = itemData.ItemType,
            LastUpdateDate = null,
            LevelRequirement = itemData.LevelRequirement,
            Tier = itemData.LevelRequirement,
            Rarity = itemData.LevelRequirement,
            Perks = itemData.Perks,
            IsLimitedToVerifiedUsers = false, //TODO: implement verified user stuff
            ShortId = ShortId.Generate(new GenerationOptions(true,false,8))
        };

        _unitOfWork.OrderRepository.Insert(oq);
        var saveResult = _unitOfWork.Save();
        return saveResult;
    }

    public Result CreateBuyOrder(CreateBuyOrder request)
    {
        throw new NotImplementedException();
    }

    public Result CancelOrder(Guid userGuid, Guid orderGuid)
    {
        var order = _unitOfWork.OrderRepository.GetFirstOrDefault(x => x.Guid == orderGuid,"Character");
        if (order == null)
            return DomainResult.Order.ErrNotFound;
        var isOwner = order.Character.UserGuid == userGuid;
        if (!isOwner)
            return DomainResult.Unauthorized;
        var isCompleted = order.CompletedDate.HasValue;
        if (isCompleted)
            return DomainResult.Order.ErrCompleted;
        var isCancelled = order.CancelledDate.HasValue;
        if (isCancelled)
            return DomainResult.Order.ErrCancelled;
        var isExpired = order.ExpirationDate < DateTime.Now;
        if (isExpired)
            return DomainResult.Order.ErrExpired;
        order.CancelledDate = DateTime.Now;
        _unitOfWork.OrderRepository.Update(order);
        var saveResult = _unitOfWork.Save();
        return saveResult;
    }

    public Result ActivateExpiredOrder(Guid userGuid, Guid orderRequestGuid)
    {
        var order = _unitOfWork.OrderRepository.GetFirstOrDefault(x => x.Guid == orderRequestGuid);
        if (order == null)
            return DomainResult.Order.ErrNotFound;
        var isOwner = order.Character.UserGuid == userGuid;
        if (!isOwner)
            return DomainResult.Unauthorized;
        var isCompleted = order.CompletedDate.HasValue;
        if (isCompleted)
            return DomainResult.Order.ErrCompleted;
        var isCancelled = order.CancelledDate.HasValue;
        if (isCancelled)
            return DomainResult.Order.ErrCancelled;
        var isExpired = order.ExpirationDate < DateTime.Now;
        if (isExpired)
            return DomainResult.Order.ErrExpired;
        order.ExpirationDate = DateTime.Now.AddHours(ConstMgr.DefaultExpirationTimeInHours);
        _unitOfWork.OrderRepository.Update(order);
        var saveResult = _unitOfWork.Save();
        return saveResult;
    }
   
    public ResultData<List<Order>> GetMainPageSellOrders(int region = -1, int server = -1, int page = 1)
    {
        if (region > 0)
        {
            return _unitOfWork.OrderRepository.GetPaging(
                                   page,
                                                      ConstMgr.PageSize,
                                                      x => x.Type == 1 && x.Region == region && x.Server == server && !x.CancelledDate.HasValue && !x.CompletedDate.HasValue && x.ExpirationDate > DateTime.Now,
                                                      x => x.OrderByDescending(y => y.RegisterDate))
                .Include(x => x.Character)
                .ToList();
        }

        return _unitOfWork.OrderRepository.GetPaging(
                page,
                ConstMgr.PageSize,
                x => x.Type == 1 && !x.CancelledDate.HasValue && !x.CompletedDate.HasValue && x.ExpirationDate > DateTime.Now,
                x => x.OrderByDescending(y => y.RegisterDate))
            .Include(x => x.Character)

            .ToList();
    }

    public ResultData<List<Order>> GetMainPageBuyOrders(int region = -1, int server = -1, int page = 1)
    {
        if (region > 0)
        {
            return _unitOfWork.OrderRepository.GetPaging(
                    page,
                    ConstMgr.PageSize,
                    x => x.Type == 0 && x.Region == region && x.Server == server && !x.CancelledDate.HasValue && !x.CompletedDate.HasValue && x.ExpirationDate > DateTime.Now,
                    x => x.OrderByDescending(y => y.RegisterDate))
                .Include(x => x.Character)
                .ToList();
        }
        return _unitOfWork.OrderRepository.GetPaging(
                           page,
                                          ConstMgr.PageSize,
                                          x => x.Type == 0 && !x.CancelledDate.HasValue && !x.CompletedDate.HasValue && x.ExpirationDate > DateTime.Now,
                                          x => x.OrderByDescending(y => y.RegisterDate))
            .Include(x => x.Character)
            .ToList();
       
    }

    public ResultData<List<Order>> GetOrdersByUsername(byte type, string username, int page)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Username == username);
        if (user is null)
            return Result.Error("User not found");
        var characterGuidList = user.Characters.Select(x => x.Guid).ToList();
        return _unitOfWork.OrderRepository.GetPaging(
                page,
                ConstMgr.PageSize,
                x => x.Type == type && characterGuidList.Contains(x.CharacterGuid),
                x => x.OrderByDescending(y => y.RegisterDate))
            .ToList();
    }

    public ResultData<List<Order>> GetOrdersByUserGuid(byte type, Guid userGuid)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == userGuid);
        if (user is null)
            return Result.Error("User not found");
        var characterGuidList = user.Characters.Select(x => x.Guid).ToList();
        return _unitOfWork.OrderRepository.GetOrdered(
                x => x.Type == type && characterGuidList.Contains(x.CharacterGuid),
                x => x.OrderByDescending(y => y.RegisterDate))
            .ToList();
    }

    public ResultData<ActiveOrderData> GetUserOrders(Guid userGuid)
    {
        throw new NotImplementedException();
    }

    public ResultData<List<Order>> GetCancelledOrdersByUserGuid(Guid userGuid)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == userGuid);
        if (user is null)
            return Result.Error("User not found");
        
        var characterGuidList = user.Characters.Select(x => x.Guid).ToList();
        return _unitOfWork.OrderRepository.GetOrdered(
                           x => x.Type == 2 && characterGuidList.Contains(x.CharacterGuid),
                                          x => x.OrderByDescending(y => y.RegisterDate))
            .ToList();
    }

    public ResultData<List<Order>> GetCompletedOrdersByUserGuid(Guid userGuid)
    {
        throw new NotImplementedException();
    }

    public ResultData<List<Order>> GetExpiredOrdersByUserGuid(Guid userGuid)
    {
        throw new NotImplementedException();
    }

    public ResultData<OrderData> GetUserOrderData(Guid userGuid)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == userGuid);
        if (user is null)
            return Result.Error("User not found");
        var characters = _unitOfWork.CharacterRepository
            .Get(x => x.UserGuid == userGuid && !x.DeletedDate.HasValue)
            .Select(x => x.Guid)
            .ToList();
        var allList = _unitOfWork.OrderRepository.GetOrdered(
                x => characters.Contains(x.CharacterGuid),
                x => x.OrderByDescending(y => y.RegisterDate))
            .Include(x => x.Character)
            .ToList();
        var orderData = new OrderData()
        {
            ActiveBuyOrderList = allList.Where(x => x.Type == (int)OrderType.Buy && !x.CancelledDate.HasValue && !x.CompletedDate.HasValue && x.ExpirationDate > DateTime.Now).ToList(),
            ActiveSellOrderList = allList.Where(x => x.Type == (int)OrderType.Sell && !x.CancelledDate.HasValue && !x.CompletedDate.HasValue && x.ExpirationDate > DateTime.Now).ToList(),
            CancelledOrderList = allList.Where(x => x.CancelledDate.HasValue).ToList(),
            CompletedOrderList = allList.Where(x => x.CompletedDate.HasValue).ToList(),
            ExpiredOrderList = allList.Where(x => x.ExpirationDate < DateTime.Now).ToList()
        };
        return orderData;

    }


    public ResultData<Order> GetOrderById(Guid orderGuid)
    {
        return _unitOfWork.OrderRepository.GetFirstOrDefault(
                x => x.Guid == orderGuid);
    }

    public ResultData<List<Order>> GetOrderByHash(string hash)
    {
        return _unitOfWork.OrderRepository.GetOrdered(
                x => x.Hash == hash,
                x => x.OrderByDescending(y => y.RegisterDate))
            .ToList();
    }

    public ResultData<List<Order>> GetOrderByHash(Guid userGuid, string hash)
    {
        return _unitOfWork.OrderRepository.GetOrdered(
                x => x.Hash == hash && x.Guid == userGuid,
                x => x.OrderByDescending(y => y.RegisterDate))
            .ToList();
    }



    public Result CreateOrder(CreateOrder request)
    {
        //This hash check can be improved and its not tested so it may not be working correctly
        var exists = _unitOfWork.OrderRepository.Any(x => x.CharacterGuid == request.CharacterGuid && x.Hash == request.ItemData.UniqueHash);
        if (exists)
            return Result.Warn("It looks like you already listed this item, if you think this is a mistake contact us.");
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == request.UserGuid);
        if (user == null)
            return Result.Warn("User not found.");
        var character = user.Characters.FirstOrDefault(x => x.Guid == request.CharacterGuid);
        if (character == null)
            return Result.Warn("Character not found.");
        var oq = new Order
        {
            Guid = Guid.NewGuid(),
            CharacterGuid = request.CharacterGuid,
            Type = request.Type,
            Region = character.Region,
            Server = character.Server,
            Hash = request.ItemData.UniqueHash,
            Price = request.Price,
            RegisterDate = DateTime.Now,
            IsNamed = request.ItemData.IsNamed ?? false,
            GemId = request.ItemData.GemId,
            Attributes = request.ItemData.Attributes,
            CancelledDate = null,
            CompletedDate = null,
            ImageGuid = request.ImageGuid,
            EstimatedDeliveryTimeHour = request.EstimatedDeliveryTimeHour,
            ExpirationDate = DateTime.Now.AddDays(14),
            GearScore = request.ItemData.GearScore,
            IsGemChangeable = request.ItemData.IsGemChangeable,
            IsValid = true,
            ItemType = request.ItemData.ItemType,
            LastUpdateDate = null,
            LevelRequirement = request.ItemData.LevelRequirement,
            Tier = request.ItemData.LevelRequirement,
            Rarity = request.ItemData.LevelRequirement,
            Perks = request.ItemData.Perks,
            
        };


        _unitOfWork.OrderRepository.Insert(oq);
        var saveResult = _unitOfWork.Save();
        return saveResult;
        //if (saveResult.IsFailure)
        //    return saveResult;
        //return Result.Success();
    }



    



    public Result CreateOrderRequest(CreateOrderRequest request)
    {
        var order = _unitOfWork.OrderRepository.GetFirstOrDefault(x => x.Guid == request.OrderGuid);
        if (order == null)
            return Result.Warn("Order not found.");
        if (!order.IsViewable)
            return Result.Warn("This order is not available.");
        var requesterUser = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == request.UserGuid);
        if (requesterUser == null)
            return Result.Warn("User not found.");
        var characters = requesterUser.Characters.Select(x => x.Guid).ToList();
        var isSelfList = characters.Contains(order.CharacterGuid);
        if (isSelfList)
            return Result.Warn("You cannot create request for your own order.");
        if (order.IsLimitedToVerifiedUsers && !requesterUser.IsVerifiedAccount)
        {
            return Result.Warn("This order is limited to verified users.");
        }
        var orderListerCharacter = _unitOfWork.CharacterRepository.GetFirstOrDefault(x => x.Guid == order.CharacterGuid);
        if (orderListerCharacter == null)
            return Result.Warn("Order lister user not found.");
        var orderListerUser = orderListerCharacter.User;

        var exists = _unitOfWork.OrderRequestRepository
            .Any(x => x.OrderGuid == order.Guid 
                      && (x.CharacterGuid == request.CharacterGuid || 
                          x.Character.UserGuid == request.UserGuid));
        if (exists)
            return Result.Warn("You already have a pending request for this order.");

        var orderRequest = new OrderRequest
        {
            Guid = Guid.NewGuid(),
            OrderGuid = order.Guid,
            CharacterGuid = request.CharacterGuid,
            IsCompletionVerifiedByOrderOwner = false,
            IsCompletionVerifiedByRequester = false,
            RegisterDate = DateTime.Now,
            CancelDate = null
        };
        _unitOfWork.OrderRequestRepository.Insert(orderRequest);
        var saveResult = _unitOfWork.Save();
        //Todo: send email verification
        return saveResult;
    }

    public Result CancelOrderRequest(Guid userGuid, Guid orderRequestGuid)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == orderRequestGuid);
        if (user == null)
            return Result.Warn("User not found.");
        var characters = user.Characters.Select(x => x.Guid).ToList();
        var orderRequest = _unitOfWork.OrderRequestRepository
            .GetFirstOrDefault(x =>
                characters.Contains(x.CharacterGuid) &&
                x.OrderGuid == orderRequestGuid &&
                !x.CancelDate.HasValue);
        if (orderRequest == null)
            return Result.Warn("Order request not found.");
        //var isOwner = orderRequest.Character.UserGuid == userGuid;
        //if (!isOwner)
        //    return DomainResult.Unauthorized;

        //if (orderRequest.Order is null)
        //{
        //    //??
        //    orderRequest.Order = _unitOfWork.OrderRepository.GetFirstOrDefault(x => x.Guid == orderRequest.OrderGuid);
        //    if (orderRequest.Order == null)
        //        return Result.Warn("Order not found.");
        //}
        if (orderRequest.IsCancelled)
            return Result.Warn("Order request is cancelled already");
        if (orderRequest.IsCompleted)
            return Result.Warn("You can not cancel a completed order request.");
        orderRequest.CancelDate = DateTime.Now;
        _unitOfWork.OrderRequestRepository.Update(orderRequest);
        var res = _unitOfWork.Save();
        return res;
    }

   


}