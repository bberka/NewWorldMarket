using EasMe.Extensions;
using EasMe.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewWorldMarket.Core.Constants;
using NuGet.Protocol;
using static Google.Cloud.RecaptchaEnterprise.V1.AccountVerificationInfo.Types;
using Result = EasMe.Result.Result;

namespace NewWorldMarket.Web.Controllers.ApiControllers;

[Authorize]
public class OrderController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IOrderReportService _orderReportService;
    private readonly IFileLogger _fileLogger;
    private readonly ILogService _logService;

    public OrderController(
        IOrderService orderService,
        IOrderReportService orderReportService,
        IFileLogger fileLogger,
        ILogService logService)
    {
        _orderService = orderService;
        _orderReportService = orderReportService;
        _fileLogger = fileLogger;
        _logService = logService;
    }


    [HttpGet]
    public IActionResult CancelOrder(Guid guid)
    {
        var user = SessionLib.This.GetUser()!;
        var result = _orderService.CancelOrder(user.Guid, guid);
        _fileLogger.Log(ActionType.OrderCancel, result.Severity, result.ErrorCode, guid);
        _logService.Log(ActionType.OrderCancel, result.Severity, result.ErrorCode, guid);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult CompleteOrder(Guid guid)
    {
        var user = SessionLib.This.GetUser()!;
        var result = _orderService.CompleteOrder(user.Guid, guid);
        _fileLogger.Log(ActionType.OrderComplete, result.Severity, result.ErrorCode, guid);
        _logService.Log(ActionType.OrderComplete, result.Severity, result.ErrorCode, guid);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult ActivateOrder(Guid guid)
    {
        var user = SessionLib.This.GetUser()!;
        var result = _orderService.ActivateExpiredOrder(user.Guid, guid);
        _fileLogger.Log(ActionType.OrderActivate, result.Severity, result.ErrorCode, guid);
        _logService.Log(ActionType.OrderActivate, result.Severity, result.ErrorCode, guid);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult UpdateOrderPrice(Guid guid, float price)
    {
        var user = SessionLib.This.GetUser()!;
        var result = _orderService.UpdateOrderPrice(user.Guid, guid, price);
        _fileLogger.Log(ActionType.OrderUpdate, result.Severity, result.ErrorCode, guid);
        _logService.Log(ActionType.OrderUpdate, result.Severity, result.ErrorCode, guid);
        return Ok(result);
    }
    [HttpPost]
    [AllowAnonymous]
    public ActionResult<Result> Report(CreateOrderReport report)
    {
        var info = HttpContext.GetInfo();
        var result = _orderReportService.CreateReport(SessionLib.This.GetUser()?.Guid, report, info);
        _fileLogger.Log(ActionType.OrderReport, result.Severity, result.ErrorCode,report);
        _logService.Log(ActionType.OrderReport, result.Severity, result.ErrorCode,report);
        return Ok(result);
    }
    //[HttpGet]
    //public IActionResult GetSellOrders(
    //    [FromQuery] int attr = -1,
    //    [FromQuery] int perk1 = -1,
    //    [FromQuery] int perk2 = -1,
    //    [FromQuery] int perk3 = -1,
    //    [FromQuery] int item = -1,
    //    [FromQuery] int server = -1,
    //    [FromQuery] int rarity = -1
    //)
    //{
    //    var result = _orderService.GetFilteredActiveOrders(attr, perk1, perk2, perk3, item, server, rarity);
    //    return Ok(Serializer.ToJson(result.Data));
    //}
}