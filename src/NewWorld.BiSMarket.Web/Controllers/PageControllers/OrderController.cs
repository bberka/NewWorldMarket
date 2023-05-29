using EasMe.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Web.Controllers.PageControllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IImageService _imageService;

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
            if (image.IsFailure)
            {
                return RedirectToAction("UploadItemImage", "Order");
            }
            var user = SessionLib.This.GetUser();
            var isOwner = image.Data?.UserGuid == user?.Guid;
            if (!isOwner)
            {
                return RedirectToAction("UploadItemImage", "Order");
            }
            var item = image.Data?.OcrItemDataResult.FromJsonString<Item>();
            if (item is null)
            {
                return RedirectToAction("UploadItemImage", "Order");
            }
            var characters = _userService.GetCharacters(user.Guid);
            var createBuyOrder = new CreateSellOrder()
            {
                CharactersListForView = characters.Data ?? new(),
                GearScore = item.GearScore,
                Attributes = item.Attributes,
                IsNamed = item.IsNamed ?? false,
                ItemType = item.ItemType,
                LevelRequirement = item.LevelRequirement,
                IsGemChangeable = item.IsGemChangeable,
                GemId = item.GemId,
                Rarity = item.Rarity,
                Tier = item.Tier,
                Perks = item.Perks,
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
                request.CharactersListForView = characters.Data ?? new();
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
                CharactersListForView = characters.Data ?? new(),
            };
            return View(createBuyOrder);
        }
        [HttpPost]
        public IActionResult CreateBuyOrder(CreateBuyOrder request)
        {

            throw new System.NotImplementedException();
        }
    }
}
