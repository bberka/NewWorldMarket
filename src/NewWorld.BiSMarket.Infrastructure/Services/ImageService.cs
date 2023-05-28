using EasMe.Result;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Core.Entity;

namespace NewWorld.BiSMarket.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly IUnitOfWork _unitOfWork;

    public ImageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public ResultData<Image> GetImage(Guid guid)
    {
        return _unitOfWork.ImageRepository.GetById(guid);
    }
}