using EasMe.Extensions;
using EasMe.Result;
using Microsoft.AspNetCore.Http;
using NewWorld.BiSMarket.Core;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Core.Constants;
using NewWorld.BiSMarket.Core.Entity;
using NewWorld.BiSMarket.Core.Models;
using System.Drawing;
using Image = NewWorld.BiSMarket.Core.Entity.Image;

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
    public ResultData<Guid> UploadItemImageAndGetImageGuid(Guid userGuid, IFormFile file)
    {
        if (file is null)
        {
            return Result.Fatal("File can not be empty");
        }
        switch (file.Length)
        {
            case < 1:
                return Result.Warn("File size can not be zero.");
            case > ConstMgr.MaxImageSize:
                return Result.Warn("File size can not be bigger than 1MB.");
        }

        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var fileBytes = ms.ToArray();
        var ocr = ItemImageOcr.Create(fileBytes);
        var readResult = ocr.Read(out var ocrTextResult);
        if (readResult.IsFailure)
        {
            return Result.Warn(readResult.ErrorCode, readResult.Errors);
        }
        Bitmap bmpImage = new Bitmap(ms);
        var rect = new Rectangle(0, 0, 140, 140);
        var icon = bmpImage.Clone(rect, bmpImage.PixelFormat);
        using var iconStream = new MemoryStream();
        icon.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png);
        var iconBytes = iconStream.ToArray();
        var dbImage = new Image
        {
            Guid = Guid.NewGuid(),
            Bytes = fileBytes,
            RegisterDate = DateTime.Now,
            ContentType = file.ContentType,
            Name = file.FileName,
            OcrTextResult = ocrTextResult,
            OcrItemDataResult = readResult.Data.ToJsonString(),
            UserGuid = userGuid,
            SmallIconBytes = iconBytes
        };
        _unitOfWork.ImageRepository.Insert(dbImage);
        var saveResult = _unitOfWork.Save();
        if (saveResult.IsFailure)
        {
            return Result.Warn(saveResult.ErrorCode, saveResult.Errors);
        }
        return dbImage.Guid;
    }

    public ResultData<Item> GetItemData(Guid imageGuid)
    {
        var image = _unitOfWork.ImageRepository.GetById(imageGuid);
        if (image is null)
        {
            return Result.Error("Image not found");
        }
        return image.OcrItemDataResult.FromJsonString<Item>();
    }
}