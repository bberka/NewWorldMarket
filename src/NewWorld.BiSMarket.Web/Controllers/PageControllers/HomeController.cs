using Microsoft.AspNetCore.Mvc;
using NewWorld.BiSMarket.Web.Models;
using System.Diagnostics;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Web.Controllers.PageControllers
{
    public class HomeController : Controller
    {
        private readonly IOrderService _orderService;

        public HomeController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Route("/")]

        public IActionResult Index()
        {
            var buyOrders = _orderService.GetMainPageBuyOrders();
            var sellOrders = _orderService.GetMainPageSellOrders();
            if (buyOrders.IsSuccess && sellOrders.IsSuccess)
            {
                return View(new OrderData
                {
                    BuyOrderList = buyOrders.Data!,
                    SellOrderList = sellOrders.Data!
                });
            }
            return View(new OrderData());
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
        public IActionResult ContactUs()
        {

            return View();
        }
    }
}