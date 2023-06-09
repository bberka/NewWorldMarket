using Microsoft.AspNetCore.Mvc;
using NewWorldMarket.Core.Constants;

namespace NewWorldMarket.Web.Controllers.ApiControllers;

public class ImageController : BaseApiController
{
    private readonly IImageService _imageService;
    private readonly IFileLogger _fileLogger;
    private readonly ILogService _logService;

    public ImageController(
        IImageService imageService,
        IFileLogger fileLogger,
        ILogService logService)
    {
        _imageService = imageService;
        _fileLogger = fileLogger;
        _logService = logService;
    }

    [HttpGet]
    [Route("/api/Image/Get/{guid}")]
    [ResponseCache(Duration = 6000,VaryByQueryKeys = new[]{ "guid"})]
    public IActionResult Get(Guid guid)
    {
        var result = _imageService.GetImage(guid);
        //_fileLogger.Log(ActionType.ImageGet, result.Severity, result.ErrorCode);
        //_logService.Log(ActionType.ImageGet, result.Severity, result.ErrorCode);
        if (result.IsSuccess) return File(result.Data!.Bytes, result.Data.ContentType);
        return BadRequest();
    }

    [HttpGet]
    [Route("/api/Image/GetIcon/{guid}")]
    [ResponseCache(Duration = 6000, VaryByQueryKeys = new[] { "guid" })]
    public IActionResult GetIcon(Guid guid)
    {
        var result = _imageService.GetImage(guid);
        _fileLogger.Log(ActionType.ImageGet, result.Severity, result.ErrorCode);
        _logService.Log(ActionType.ImageGet, result.Severity, result.ErrorCode);
        if (result.IsSuccess) return File(result.Data!.SmallIconBytes, result.Data.ContentType);
        return BadRequest();
    }
}