using EasMe.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Core.Entity;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Web.Controllers.PageControllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IImageService _imageService;
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;

    public OrderController(
        IUserService userService,
        IOrderService orderService,
        IImageService imageService)
    {
        _userService = userService;
        _orderService = orderService;
        _imageService = imageService;
    }

    [HttpGet]
    public IActionResult UploadItemImage()
    {
        return View();
    }

    [HttpPost]
    public IActionResult UploadItemImage(UploadImage image)
    {
        var user = SessionLib.This.GetUser();
        var uploadResult = _imageService.UploadItemImageAndGetImageGuid(user.Guid, image.FormFile);
        if (uploadResult.IsFailure)
        {
            ModelState.AddModelError("", uploadResult.ErrorCode);

            //foreach (var item in uploadResult.Errors)
            //{
            //    ModelState.AddModelError("", item);
            //}
            return View();
        }

        return RedirectToAction("CreateSellOrder", new
        {
            imageGuid = uploadResult.Data
        });
    }

    [HttpGet]
    [Route("/[controller]/[action]/{imageGuid}")]
    public IActionResult CreateSellOrder(Guid imageGuid)
    {
        var image = _imageService.GetImage(imageGuid);
        if (image.IsFailure) return RedirectToAction("UploadItemImage", "Order");
        var user = SessionLib.This.GetUser();
        var isOwner = image.Data?.UserGuid == user?.Guid;
        if (!isOwner) return RedirectToAction("UploadItemImage", "Order");
        var item = image.Data?.OcrItemDataResult.FromJsonString<ItemV3>();
        if (item is null) return RedirectToAction("UploadItemImage", "Order");
        var characters = _userService.GetCharacters(user.Guid);
        var createBuyOrder = new CreateSellOrder
        {
            CharactersListForView = characters.Data ?? new List<Character>(),
            GearScore = item.GearScore,
            Attributes = item.AttributeString,
            IsNamed = item.IsNamed ?? false,
            ItemType = item.ItemType,
            LevelRequirement = item.LevelRequirement,
            IsGemChangeable = true,
            GemId = item.GemId,
            Rarity = item.Rarity,
            Tier = item.Tier,
            Perks = item.PerkString,
            UserGuid = user.Guid,
            ImageGuid = imageGuid,
            ImageBytesBase64String = image.Data.Bytes.ToBase64String()
        };
        return View(createBuyOrder);
    }

    [HttpPost]
    public IActionResult CreateSellOrder(CreateSellOrder request)
    {
        var user = SessionLib.This.GetUser();
        request.UserGuid = user.Guid;
        var result = _orderService.CreateSellOrder(request);
        if (result.IsFailure)
        {
            var characters = _userService.GetCharacters(user.Guid);
            request.CharactersListForView = characters.Data ?? new List<Character>();
            ModelState.AddModelError("", result.ErrorCode);
            return View(request);
        }

        return RedirectToAction("MyOrders", "Account");
    }


    [HttpGet]
    public IActionResult CreateBuyOrder()
    {
        var user = SessionLib.This.GetUser();
        var characters = _userService.GetCharacters(user.Guid);
        var createBuyOrder = new CreateBuyOrder
        {
            CharactersListForView = characters.Data ?? new List<Character>()
        };
        return View(createBuyOrder);
    }

    [HttpPost]
    public IActionResult CreateBuyOrder(CreateBuyOrder request)
    {
        throw new NotImplementedException();
    }
}