using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewWorldMarket.Web.Controllers.PageControllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;

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
        var user = SessionLib.This.GetUser();
        var result = _userService.ChangePassword(user!.Guid, request);
        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorCode);
            return View(request);
        }

        return RedirectToAction("Logout", "Account");
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
}