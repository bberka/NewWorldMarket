using EasMe.Result;

namespace NewWorld.BiSMarket.Core.Abstract;

public interface IImageService
{
    ResultData<Entity.Image> GetImage(Guid guid);

}