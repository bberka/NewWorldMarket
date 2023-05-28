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
using Image = NewWorld.BiSMarket.Core.Entity.Image;

namespace NewWorld.BiSMarket.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public ResultData<List<Order>> GetMainPageSellOrders(int region = -1, int server = -1, int page = 1)
    {
        if (region > 0)
        {
            return _unitOfWork.OrderRepository.GetPaging(
                                   page,
                                                      ConstMgr.PageSize,
                                                      x => x.Type == 1 && x.Region == region && x.Server == server,
                                                      x => x.OrderByDescending(y => y.RegisterDate))
                .ToList();
        }

        return _unitOfWork.OrderRepository.GetPaging(
                page,
                ConstMgr.PageSize,
                x => x.Type == 1,
                x => x.OrderByDescending(y => y.RegisterDate))
            .ToList();
    }

    public ResultData<List<Order>> GetMainPageBuyOrders(int region = -1, int server = -1, int page = 1)
    {
        if (region > 0)
        {
            return _unitOfWork.OrderRepository.GetPaging(
                    page,
                    ConstMgr.PageSize,
                    x => x.Type == 0 && x.Region == region && x.Server == server,
                    x => x.OrderByDescending(y => y.RegisterDate))
                .ToList();
        }
        return _unitOfWork.OrderRepository.GetPaging(
                           page,
                                          ConstMgr.PageSize,
                                          x => x.Type == 0,
                                          x => x.OrderByDescending(y => y.RegisterDate))
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
                x => x.Type == type && characterGuidList.Contains(x.CharacterGuid.Value),
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
                x => x.Type == type && characterGuidList.Contains(x.CharacterGuid.Value),
                x => x.OrderByDescending(y => y.RegisterDate))
            .ToList();
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
            IsEmptySocket = request.ItemData.IsEmptySocket,
            IsNamed = request.ItemData.IsEmptySocket,
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

    public Result CancelOrder(CancelOrder request)
    {
        var order = _unitOfWork.OrderRepository.GetFirstOrDefault(x => x.Guid == request.OrderGuid);
        if (order == null)
            return Result.Warn("Order not found.");
        if (order.CompletedDate != null)
            return Result.Warn("This order is already completed.");
        if (order.CancelledDate != null)
            return Result.Warn("This order is already cancelled.");
        order.CancelledDate = DateTime.Now;
        _unitOfWork.OrderRepository.Update(order);
        var saveResult = _unitOfWork.Save();
        return saveResult;
    }

    public Result ActivateExpiredOrder(ActivateExpiredOrder request)
    {
        var order = _unitOfWork.OrderRepository.GetFirstOrDefault(x => x.Guid == request.OrderGuid);
        if (order == null)
            return Result.Warn("Order not found.");
        if (order.CompletedDate != null)
            return Result.Warn("This order is already completed.");
        if (order.CancelledDate != null)
            return Result.Warn("This order is already cancelled.");
        if(order.ExpirationDate > DateTime.Now)
            return Result.Warn("This order is not expired yet.");
        order.ExpirationDate = DateTime.Now.AddDays(14);
        _unitOfWork.OrderRepository.Update(order);
        var saveResult = _unitOfWork.Save();
        return saveResult;

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
        var isSelfList = characters.Contains(order.CharacterGuid.Value);
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

    public Result CancelOrderRequest(CancelOrderRequest request)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == request.UserGuid);
        if (user == null)
            return Result.Warn("User not found.");
        var characters = user.Characters.Select(x => x.Guid).ToList();

        var orderRequest = _unitOfWork.OrderRequestRepository
            .GetFirstOrDefault(x => 
                characters.Contains(x.CharacterGuid) && 
                x.OrderGuid == request.OrderGuid && 
                !x.CancelDate.HasValue);
        if (orderRequest == null)
            return Result.Warn("Order request not found.");

        //if (orderRequest.Order is null)
        //{
        //    //??
        //    orderRequest.Order = _unitOfWork.OrderRepository.GetFirstOrDefault(x => x.Guid == orderRequest.OrderGuid);
        //    if (orderRequest.Order == null)
        //        return Result.Warn("Order not found.");
        //}
        if(orderRequest.IsCancelled)
            return  Result.Warn("Order request is cancelled already");
        if(orderRequest.IsCompleted)
            return Result.Warn("You can not cancel a completed order request.");
        orderRequest.CancelDate = DateTime.Now;
        _unitOfWork.OrderRequestRepository.Update(orderRequest);
        var res = _unitOfWork.Save();
        return res;
    }

    public ResultData<Item> UploadItemImageAndGetItemData(IFormFile file)
    {
        switch (file.Length)
        {
            case < 1:
                return Result.Warn("File size can not be zero.");
            case > ConstMgr.MaxImageSize:
                return Result.Warn("File size can not be bigger than 1MB.");
        }

        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var fileBytes = ms.ToArray();
        var ocr = ItemImageOcr.Create(fileBytes);
        var readResult = ocr.Read(out var ocrTextResult);
        if (readResult.IsFailure)
        {
            return Result.Warn(readResult.ErrorCode,readResult.Errors);
        }
        var dbImage = new Image
        {
            Guid = Guid.NewGuid(),
            Bytes = fileBytes,
            RegisterDate = DateTime.Now,
            ContentType  = file.ContentType,
            Name = file.FileName,
            OcrTextResult = ocrTextResult,
            OcrItemDataResult = readResult.Data.ToJsonString()
        };
        _unitOfWork.ImageRepository.Insert(dbImage);
        var saveResult = _unitOfWork.Save();
        if (saveResult.IsFailure)
        {
            return Result.Warn(saveResult.ErrorCode,saveResult.Errors);
        }
        return readResult.Data;
    }
}