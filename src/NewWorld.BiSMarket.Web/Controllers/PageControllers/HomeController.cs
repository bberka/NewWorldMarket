using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Core.Models;
using NewWorld.BiSMarket.Web.Models;

namespace NewWorld.BiSMarket.Web.Controllers.PageControllers;

public class HomeController : Controller
{
    private readonly IOrderService _orderService;

    public HomeController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    //[Route("/")]
    //[HttpGet]
    //public IActionResult Index()
    //{
    //    var buyOrders = _orderService.GetMainPageBuyOrders();
    //    var sellOrders = _orderService.GetMainPageSellOrders();
    //    if (buyOrders.IsSuccess && sellOrders.IsSuccess)
    //    {
    //        return View(new ActiveOrderData
    //        {
    //            BuyOrderList = buyOrders.Data!,
    //            SellOrderList = sellOrders.Data!
    //        });
    //    }
    //    return View(new ActiveOrderData());
    //}
    [Route("/List")]
    [Route("/")]
    [HttpGet]
    public IActionResult Index(
        [FromQuery] int attr = -1,
        [FromQuery] int perk1 = -1,
        [FromQuery] int perk2 = -1,
        [FromQuery] int perk3 = -1,
        [FromQuery] int item = -1,
        [FromQuery] int server = -1,
        [FromQuery] int rarity = -1
    )
    {
        var model = new ActiveOrderData();
        var result = _orderService.GetFilteredActiveOrders(attr, perk1, perk2, perk3, item, server, rarity);
        if (result.IsSuccess) model.SellOrderList = result.Data!;
        return View(model);
    }

    [Route("/{statusCode}")]
    public IActionResult ErrorCode(int statusCode)
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    [Route("/Contact")]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpGet]
    [Route("/FAQ")]
    public IActionResult FAQ()
    {
        return View();
    }
    [HttpGet]
    [Route("/Donation")]
    public IActionResult Donation()
    {
        return View();
    }
    [HttpGet]
    [Route("/About")]
    public IActionResult About()
    {
        return View();
    }
}