using EasMe;
using EasMe.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using NewWorldMarket.Web.Models;

namespace NewWorldMarket.Web.Controllers.PageControllers;

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
        return RedirectToAction("Index", "Home");
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
        return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpGet]
    [Route("/Discord")]
    public IActionResult Discord()
    {
        var discordUrl = EasConfig.GetString("DiscordUrl");
        if (discordUrl.IsNullOrEmpty()) return RedirectToAction("Index");
        return Redirect(discordUrl!);
    }
    [HttpGet]
    [Route("/Example")]
    public IActionResult Example()
    {
        return View();
    }
    //[HttpGet]
    //[Route("/About")]
    //public IActionResult About()
    //{
    //    return View();
    //}
    //[HttpGet]
    //[Route("/TermsOfService")]
    //public IActionResult TermsOfService()
    //{
    //    return View();
    //}
    //[HttpGet]
    //[Route("/PrivacyPolicy")]
    //public IActionResult PrivacyPolicy()
    //{
    //    return View();
    //}
}