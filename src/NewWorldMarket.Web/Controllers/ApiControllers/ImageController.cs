using Microsoft.AspNetCore.Mvc;

namespace NewWorldMarket.Web.Controllers.ApiControllers;

public class ImageController : BaseApiController
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpGet]
    [Route("/api/Image/Get/{guid}")]
    [ResponseCache(Duration = 6000,VaryByQueryKeys = new[]{ "guid"})]
    public IActionResult Get(Guid guid)
    {
        var image = _imageService.GetImage(guid);
        if (image.IsSuccess) return File(image.Data!.Bytes, image.Data.ContentType);
        return BadRequest();
    }

    [HttpGet]
    [Route("/api/Image/GetIcon/{guid}")]
    [ResponseCache(Duration = 6000, VaryByQueryKeys = new[] { "guid" })]
    public IActionResult GetIcon(Guid guid)
    {
        var image = _imageService.GetImage(guid);
        if (image.IsSuccess) return File(image.Data!.SmallIconBytes, image.Data.ContentType);
        return BadRequest();
    }
}