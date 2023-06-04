using EasMe.Result;
using Microsoft.AspNetCore.Http;
using NewWorld.BiSMarket.Core.Models;
using Image = NewWorld.BiSMarket.Core.Entity.Image;

namespace NewWorld.BiSMarket.Core.Abstract;

public interface IImageService
{
    ResultData<Image> GetImage(Guid guid);
    ResultData<Guid> UploadItemImageAndGetImageGuid(Guid userGuid, IFormFile file);
    ResultData<ItemV3> GetItemData(Guid imageGuid);
}