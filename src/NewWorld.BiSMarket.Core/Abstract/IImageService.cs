using EasMe.Result;
using Microsoft.AspNetCore.Http;
using NewWorldMarket.Core.Entity;
using NewWorldMarket.Core.Models;

namespace NewWorldMarket.Core.Abstract;

public interface IImageService
{
    ResultData<Image> GetImage(Guid guid);
    ResultData<Guid> UploadItemImageAndGetImageGuid(Guid userGuid, IFormFile file);
    ResultData<ItemV3> GetItemData(Guid imageGuid);
}