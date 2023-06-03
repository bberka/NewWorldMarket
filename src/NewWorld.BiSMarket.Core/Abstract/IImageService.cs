using EasMe.Result;
using Microsoft.AspNetCore.Http;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Core.Abstract;

public interface IImageService
{
    ResultData<Entity.Image> GetImage(Guid guid);
    ResultData<Guid> UploadItemImageAndGetImageGuid(Guid userGuid,IFormFile file);
    ResultData<ItemV3> GetItemData(Guid imageGuid);

}