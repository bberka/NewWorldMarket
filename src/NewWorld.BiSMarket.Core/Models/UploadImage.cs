using Microsoft.AspNetCore.Http;

namespace NewWorldMarket.Core.Models;

public class UploadImage
{
    public IFormFile FormFile { get; set; }
}