using Microsoft.AspNetCore.Http;

namespace NewWorld.BiSMarket.Core.Models;

public class UploadImage
{
    public IFormFile FormFile { get; set; }
}