using Microsoft.AspNetCore.Mvc;

namespace NewWorld.BiSMarket.Web.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class BaseApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
