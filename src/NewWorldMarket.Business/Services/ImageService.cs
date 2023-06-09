using NewWorldMarket.Core;
using NewWorldMarket.Core.Abstract;
using NewWorldMarket.Core.Constants;
using NewWorldMarket.Entities;
using NewWorldMarket.Core.Models;
using NewWorldMarket.Core.Tools;

namespace NewWorldMarket.Business.Services;

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

    public ResultData<Guid> UploadItemImageAndGetImageGuid(Guid userGuid, IFormFile file)
    {
        if (file is null) return Result.Fatal("File can not be empty");
        switch (file.Length)
        {
            case < 1:
                return Result.Warn("File size can not be zero.");
            case > ConstMgr.MaxImageSize:
                return Result.Warn("File size can not be bigger than 1MB.");
        }

        var fileExtension = Path.GetExtension(file.FileName);
        if (fileExtension is not ".png" and not ".jpg" and not ".jpeg")
            return Result.Warn("File extension is not supported. Supported extensions png,jpg,jpeg");
        var isAbleToUpload = _unitOfWork.ImageRepository.Any(x => x.UserGuid == userGuid 
                                                                  && x.RegisterDate.AddSeconds(15) > DateTime.Now);
        if (isAbleToUpload) return Result.Warn("You are uploading images too fast, please wait and try again.");
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var fileBytes = ms.ToArray();
        var ocr = ItemImageOcrV4.Create(fileBytes);
        var readResult = ocr.Read();
        if (!readResult.IsSuccess) return Result.Warn(ErrCode.OcrReadError.ToMessage(), readResult.Errors.ToList());
        //var bmpImage = new Bitmap(ms);
        //var rect = new Rectangle(0, 0, 140, 140);
        //var icon = bmpImage.Clone(rect, bmpImage.PixelFormat);
        //using var iconStream = new MemoryStream();
        //icon.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png);
        //var iconBytes = iconStream.ToArray();
        var dbImage = new Image
        {
            Guid = Guid.NewGuid(),
            Bytes = readResult.Data.FullImageBytes,
            RegisterDate = DateTime.Now,
            ContentType = file.ContentType,
            Name = file.FileName,
            OcrTextResult = readResult.Data.OcrTextResult,
            OcrItemDataResult = readResult.Data.Item.ToJsonString(),
            UserGuid = userGuid,
            SmallIconBytes = readResult.Data.IconBytes
        };
        _unitOfWork.ImageRepository.Insert(dbImage);
        var saveResult = _unitOfWork.Save();
        if (saveResult.IsFailure) return Result.Warn(saveResult.ErrorCode, saveResult.Errors);
        return dbImage.Guid;
    }

    public ResultData<ItemV3> GetItemData(Guid imageGuid)
    {
        var image = _unitOfWork.ImageRepository.GetById(imageGuid);
        if (image is null) return Result.Error("Image not found");
        return image.OcrItemDataResult.FromJsonString<ItemV3>();
    }
}