using EasMe.Result;
using Microsoft.AspNetCore.Mvc;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Core.Entity;

namespace NewWorld.BiSMarket.Web.Controllers.ApiControllers
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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

    }
}
