using EasMe;
using EasMe.Extensions;
using EasMe.Result;
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
            SessionLib.This.SetUser(registerResult.Data);
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
            var orderData = _orderService.GetUserOrderData(user!.Guid);
            return View(orderData.Data);
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
            var characters = _userService.GetCharacters(user!.Guid);
            return View(characters.Data);
        }

        [HttpGet]
        public IActionResult AddCharacter()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCharacter(AddCharacter request)
        {
            var user = SessionLib.This.GetUser();
            request.UserGuid = user!.Guid;
            var registerResult = _userService.AddCharacter(request);
            if (registerResult.IsFailure)
            {
                ModelState.AddModelError("", registerResult.ErrorCode);
                return View(request);
            }
            return RedirectToAction("Characters", "Account"); //redirect to account page

        }
        [HttpGet]
        [Route("/[controller]/[action]/{guid}")]
        public IActionResult RemoveCharacter(Guid guid)
        {
            var user = SessionLib.This.GetUser();
            var registerResult = _userService.RemoveCharacter(user.Guid,guid);
            return Ok(registerResult);

        }

    }
}
