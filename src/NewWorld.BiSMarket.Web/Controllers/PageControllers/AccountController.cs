using EasMe;
using EasMe.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Web.Controllers.PageControllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public AccountController(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            SessionLib.This.ClearSession();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(Login request)
        {
            var loginResult = _userService.Login(request.Username, request.Password);
            if (loginResult.IsFailure)
            {
                ModelState.AddModelError("", loginResult.ErrorCode);
                return View(request);
            }
            SessionLib.This.SetUser(loginResult.Data);
            return RedirectToAction("Index", "Home"); //redirect to account page
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            SessionLib.This.ClearSession();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(Register request)
        {
            var registerResult = _userService.Register(request);
            if (registerResult.IsFailure)
            {
                ModelState.AddModelError("", registerResult.ErrorCode);
                return View(request);
            }
            var loginResult = _userService.Login(request.Username, request.Password);
            if (loginResult.IsFailure)
            {
                ModelState.AddModelError("", loginResult.ErrorCode);
                return View(request);
            }
            SessionLib.This.SetUser(loginResult.Data);
            return RedirectToAction("Index", "Home"); //redirect to account page

        }
        [HttpGet]
        public IActionResult Logout()
        {
            SessionLib.This.ClearSession();
            return RedirectToAction("Index", "Home"); //redirect to account page
        }
        [HttpGet]
        public IActionResult Settings()
        {
            var user = SessionLib.This.GetUser();
            return View(user);
        }
        [HttpGet]
        public IActionResult MyOrders()
        {
            var user = SessionLib.This.GetUser();
            var buyOrders = _orderService.GetOrdersByUserGuid(0, user.Guid);
            var sellOrders = _orderService.GetOrdersByUserGuid(1, user.Guid);
            var model = new OrderData()
            {
                BuyOrderList = buyOrders,
                SellOrderList = sellOrders
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePassword request)
        {
            return View();
        }
        [HttpGet]
        public IActionResult Characters()
        {
            var user = SessionLib.This.GetUser();
            return View(user.Characters);
        }
    }
}
