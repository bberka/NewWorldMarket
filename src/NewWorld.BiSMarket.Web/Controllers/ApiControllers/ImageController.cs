using Microsoft.AspNetCore.Mvc;
using NewWorld.BiSMarket.Core.Abstract;

namespace NewWorld.BiSMarket.Web.Controllers.ApiControllers
{
    public class ImageController : BaseApiController
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        [HttpGet]
        [Route("/api/Image/Get/{guid}")]
        public IActionResult Get(Guid guid)
        {
            var image = _imageService.GetImage(guid);
            if (image.IsSuccess)
            {
                return File(image.Data!.Bytes, image.Data.ContentType);
            }
            return BadRequest();
        }
    }
}
