﻿using EasMe.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewWorldMarket.Web.Controllers.ApiControllers;

public class OrderController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IOrderReportService _orderReportService;

    public OrderController(IOrderService orderService,IOrderReportService orderReportService)
    {
        _orderService = orderService;
        _orderReportService = orderReportService;
    }

    [HttpGet]
    public ActionResult<ResultData<List<Order>>> GetMainPageSellOrders(int page)
    {
        return _orderService.GetMainPageSellOrders(page: page);
    }

    [HttpGet]
    public ActionResult<ResultData<List<Order>>> GetMainPageBuyOrders(int page)
    {
        return _orderService.GetMainPageBuyOrders(page: page);
    }

    [HttpGet]
    [Authorize]
    public IActionResult CancelOrder(Guid guid)
    {
        var user = SessionLib.This.GetUser()!;
        var cancelResult = _orderService.CancelOrder(user.Guid, guid);
        return Ok(cancelResult);
    }

    [HttpGet]
    [Authorize]
    public IActionResult CompleteOrder(Guid guid)
    {
        var user = SessionLib.This.GetUser()!;
        var cancelResult = _orderService.CompleteOrder(user.Guid, guid);
        return Ok(cancelResult);
    }

    [HttpGet]
    [Authorize]
    public IActionResult ActivateOrder(Guid guid)
    {
        var user = SessionLib.This.GetUser()!;
        var cancelResult = _orderService.ActivateExpiredOrder(user.Guid, guid);
        return Ok(cancelResult);
    }

    [HttpGet]
    [Authorize]
    public IActionResult UpdateOrderPrice(Guid guid, float price)
    {
        var user = SessionLib.This.GetUser()!;
        var cancelResult = _orderService.UpdateOrderPrice(user.Guid, guid, price);
        return Ok(cancelResult);
    }
    [HttpPost]
    [AllowAnonymous]
    public ActionResult<Result> Report(CreateOrderReport report)
    {
        return _orderReportService.CreateReport(SessionLib.This.GetUser()?.Guid, report);
    }
}