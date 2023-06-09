using EasMe.Logging;
using EasMe.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewWorldMarket.Core.Constants;
using static Google.Cloud.RecaptchaEnterprise.V1.AccountVerificationInfo.Types;

namespace NewWorldMarket.Web.Controllers.PageControllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ILogService _logService;
    private readonly IFileLogger _fileLogger;
    private readonly IUserService _userService;
    private static readonly IEasLog _logger = EasLogFactory.CreateLogger();
    public AccountController(
        IUserService userService, 
        IOrderService orderService,
        ILogService logService,
        IFileLogger fileLogger)
    {
        _userService = userService;
        _orderService = orderService;
        _logService = logService;
        _fileLogger = fileLogger;
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
        var result = _userService.Login(request.Username, request.Password);
        _fileLogger.Log(ActionType.AccountLogin, result.Severity, result.ErrorCode, request.Username);
        _logService.Log(ActionType.AccountLogin, result.Severity, result.ErrorCode, request.Username);
        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorCode);
            return View(request);
        }
        SessionLib.This.SetUser(result.Data);
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
        var result = _userService.Register(request);
        _fileLogger.Log(ActionType.AccountRegister, result.Severity, result.ErrorCode, request.Username);
        _logService.Log(ActionType.AccountRegister, result.Severity, result.ErrorCode, request.Username);
        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorCode);
            return View(request);
        }

        SessionLib.This.SetUser(result.Data);
        return RedirectToAction("Index", "Home"); //redirect to account page
    }

    [HttpGet]
    public IActionResult Logout()
    {
        _fileLogger.Log(ActionType.AccountLogout);
        _logService.Log(ActionType.AccountLogout);
        SessionLib.This.ClearSession();
        return RedirectToAction("Index", "Home"); //redirect to account page
    }

    [HttpGet]
    public IActionResult Settings()
    {
        var user = SessionLib.This.GetUser();
        _fileLogger.Log(ActionType.AccountSettings);
        _logService.Log(ActionType.AccountSettings);
        return View(user);
    }

    [HttpGet]
    public IActionResult MyOrders()
    {
        var user = SessionLib.This.GetUser();
        var result = _orderService.GetUserOrderData(user!.Guid);
        _fileLogger.Log(ActionType.AccountGetOrders, result.Severity, result.ErrorCode);
        _logService.Log(ActionType.AccountGetOrders, result.Severity, result.ErrorCode);
        return View(result.Data);
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
        _fileLogger.Log(ActionType.AccountChangePassword, result.Severity, result.ErrorCode);
        _logService.Log(ActionType.AccountChangePassword, result.Severity, result.ErrorCode);
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
        var result = _userService.GetCharacters(user!.Guid);
        _fileLogger.Log(ActionType.AccountGetCharacters, result.Severity, result.ErrorCode);
        _logService.Log(ActionType.AccountGetCharacters, result.Severity, result.ErrorCode);
        return View(result.Data);
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
        var result = _userService.AddCharacter(request);
        _fileLogger.Log(ActionType.AccountAddCharacter, result.Severity, result.ErrorCode);
        _logService.Log(ActionType.AccountAddCharacter, result.Severity, result.ErrorCode);
        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorCode);
            return View(request);
        }

        return RedirectToAction("Characters", "Account"); //redirect to account page
    }
}