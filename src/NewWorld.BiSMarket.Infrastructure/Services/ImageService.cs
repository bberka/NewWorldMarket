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
        var fileExtension = Path.GetExtension(file.FileName);
        if (fileExtension is not ".png" and not ".jpg" and not ".jpeg")
        {
            return Result.Warn("File extension is not supported. Supported extensions png,jpg,jpeg");
        }
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var fileBytes = ms.ToArray();
        var ocr = ItemImageOcrV2.Create(fileBytes);
        var readResult = ocr.Read();
        if (!readResult.IsSuccess)
        {
            if (readResult.Errors.Count < 7)
            {
                var secondTry = ItemImageOcrV2.CreateForSecondTry(fileBytes);
                readResult = secondTry.Read();
                if (!readResult.IsSuccess)
                {
                    return Result.Warn(ErrCode.OcrReadError.ToMessage(), readResult.Errors.ToList());
                }
            }
            else
            {
                return Result.Warn(ErrCode.OcrReadError.ToMessage(), readResult.Errors.ToList());
            }

        }
        //var bmpImage = new Bitmap(ms);
        //var rect = new Rectangle(0, 0, 140, 140);
        //var icon = bmpImage.Clone(rect, bmpImage.PixelFormat);
        //using var iconStream = new MemoryStream();
        //icon.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png);
        //var iconBytes = iconStream.ToArray();
        var dbImage = new Image
        {
            Guid = Guid.NewGuid(),
            Bytes = readResult.ItemImageData.FullImageBytes,
            RegisterDate = DateTime.Now,
            ContentType = file.ContentType,
            Name = file.FileName,
            OcrTextResult = readResult.Pages.ToJsonString(),
            OcrItemDataResult = readResult.ItemOcrReadData.ToJsonString(),
            UserGuid = userGuid,
            SmallIconBytes = readResult.ItemImageData.IconImageBytes,
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