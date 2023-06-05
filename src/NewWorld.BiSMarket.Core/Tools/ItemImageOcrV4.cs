using System.Drawing;
using EasMe.Extensions;
using EasMe.Result;
using NewWorldMarket.Core.Constants;
using NewWorldMarket.Core.Models;
using Tesseract;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace NewWorldMarket.Core.Tools;

/// <summary>
///     Resizing and parsing full image to separate images, each image will be read separately by OCR
/// </summary>
public class ItemImageOcrV4
{
    private readonly byte[] _firstBytes;
    private readonly ItemImageData _itemImageData;
    private readonly byte[] _restBytes;

    /*
    Epic
     input.AdaptiveThreshold(1.8F);
        input.DeNoise();
     
     */


    private ItemImageOcrV4(ItemImageData itemImageData, byte[] firstBytes, byte[] restBytes)
    {
        _itemImageData = itemImageData;
        _firstBytes = firstBytes;
        _restBytes = restBytes;
        //input.Contrast(1.8F);
        //input.AdaptiveThreshold(1.45F);
        //input.DeNoise();
        //txtSonuc.Text = page.GetText();
        //input.SelectTextColors(ConstMgr.ItemTooltipTextColorList,20);
    }


    public static ItemImageOcrV4 Create(byte[] bytes)
    {
        const int imageWidth = 455;
        var resizedBytes = CommonLib.ResizeImageWidth(bytes, imageWidth, out var imageHeight);
        var rectIcon = new Rectangle(0, 0, 130, 130);
        var rectFirstPart = new Rectangle(130, 0, imageWidth - 130, 150); //changed
        var rectRest = new Rectangle(65, 145, imageWidth - 65, imageHeight - 145);
        var ms = new MemoryStream(resizedBytes);
        var fullImage = new Bitmap(ms);
        var icon = fullImage.Clone(rectIcon, fullImage.PixelFormat);
        var firstPart = fullImage.Clone(rectFirstPart, fullImage.PixelFormat);
        var rest = fullImage.Clone(rectRest, fullImage.PixelFormat);

        using var iconStream = new MemoryStream();
        icon.Save(iconStream, ImageFormat.Png);
        var iconBytes = iconStream.ToArray();

        using var firstPartStream = new MemoryStream();
        firstPart.Save(firstPartStream, ImageFormat.Png);
        var firstPartBytes = firstPartStream.ToArray();

        using var restStream = new MemoryStream();
        rest.Save(restStream, ImageFormat.Png);
        var restBytes = restStream.ToArray();

        var itemImageData = new ItemImageData
        {
            IconImageBytes = iconBytes,
            FullImageBytes = resizedBytes
        };
        return new ItemImageOcrV4(itemImageData, firstPartBytes, restBytes);
    }


    //public void SaveAsImages(string outputDirectory)
    //{
    //    _ocrInput.SaveAsImages(outputDirectory);
    //}

    public ResultData<OcrReadResult> Read()
    {
        var errorList = new List<string>();

        var ocr = new TesseractEngine("./TessData", "eng", EngineMode.Default);
        var img = Pix.LoadFromMemory(_firstBytes);
        var img2 = Pix.LoadFromMemory(_restBytes);
        var first = ocr.Process(img);
        var firstPageText = first.GetText().RemoveSpecialCharacters(":+");
        var firstLines = firstPageText.Split("\n").Select(x => x.Trim()).ToList();
        first.Dispose();
        var rest = ocr.Process(img2);
        var restPageText = rest.GetText().RemoveSpecialCharacters(":+");
        var secondPageLines = restPageText.Split("\n").Select(x => x.Trim()).ToList();
        firstLines.RemoveAll(x => x.IsNullOrEmpty());
        secondPageLines.RemoveAll(x => x.IsNullOrEmpty());

        if (!ConstMgr.IsDevelopment)
            //Or check if text ocr has Bind on Equip in it, this will prevent screenshots from marketplace to be listed
            if (!restPageText.Contains("On Equip", StringComparison.OrdinalIgnoreCase))
                return Result.Warn("This item can not be traded");

        #region READ AND CLEAR PAGES AND LINES

        //var firstPageLineBeforeLastLine = firstLines[^2];
        //var firstPageLastLine = firstLines.Last();
        var firstPageExceptLast2Lines = firstLines.Take(firstLines.Count - 2).ToList();

        #endregion


        var item = new ItemV3();
        item.ItemName = string.Join(" ", firstPageExceptLast2Lines).RemoveLineEndings().Replace("  ", " ").Trim();

        #region READ - RARITY AND IS NAMED

        var firstPageTextFixed = firstPageText.RemoveNumbers().ReplaceLineEndings(" ").Replace("  ", " ");
        foreach (var rarityType in Enum.GetValues<RarityType>())
            if (firstPageTextFixed.Contains(rarityType.ToString()))
            {
                item.Rarity = (int)rarityType;
                break;
            }

        item.IsNamed = firstPageTextFixed.Contains("Named");
        if (item.Rarity == -1) errorList.Add(ErrCode.OcrRarityParseError.ToMessage());

        #endregion

        #region READ - ITEM TYPE

        foreach (var itemType in Enum.GetValues<ItemType>())
        {
            var fixedItemName = itemType.ToString().Replace("_", " ");
            if (firstPageTextFixed.Contains(fixedItemName))
            {
                item.ItemType = (int)itemType;
                break;
            }
        }

        if (item.ItemType == -1) errorList.Add(ErrCode.OcrItemTypeParseError.ToMessage());

        #endregion

        #region READ - LEVEL REQ

        var requirementLine =
            secondPageLines.FirstOrDefault(x => x.Contains("Level ", StringComparison.OrdinalIgnoreCase));
        var textAfterLevel = requirementLine?.Split("Level", StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        var levelRequirementParseResult = int.TryParse(textAfterLevel, out var levelRequirement);
        if (!levelRequirementParseResult)
        {
            errorList.Add(ErrCode.OcrLevelRequirementParseError.ToMessage());
            item.LevelRequirement = 60;
        }
        else
        {
            item.LevelRequirement = levelRequirement;
        }

        #endregion

        #region REMOVE USELESS LINES

        secondPageLines.RemoveAll(x => x.Contains("Level ", StringComparison.OrdinalIgnoreCase));
        secondPageLines.RemoveAll(x => x.EndsWith("Durability", StringComparison.OrdinalIgnoreCase));
        secondPageLines.RemoveAll(x => x.EndsWith("Weight", StringComparison.OrdinalIgnoreCase));
        secondPageLines.RemoveAll(x => x.EndsWith("On Equip", StringComparison.OrdinalIgnoreCase));

        #endregion

        #region READ - TIER

        var tierLine = secondPageLines.FirstOrDefault(x => x.Contains("Tier ", StringComparison.OrdinalIgnoreCase));
        var tierText = tierLine?.Replace("Tier ", "").Trim();
        var tierParseResult = Enum.TryParse(typeof(TierType), tierText, true, out var tier);
        if (!tierParseResult)
        {
            errorList.Add(ErrCode.OcrTierParseError.ToMessage());
            item.Tier = (int)TierType.V;
        }
        else
        {
            item.Tier = (int)tier;
        }

        #endregion

        #region READ - PERK

        var perkNoList = new List<int>();
        var perkLines = secondPageLines
            .Where(x => x.Contains(":"))
            .Select(x =>
                x.SeparateWordsAndRemoveIfFirstWordIsShort(3).Split(':')[0].RemoveSpecialCharacters("'").RemoveNumbers()
                    .Trim())
            .ToList();
        perkLines.RemoveAll(x => x.IsNullOrEmpty());
        foreach (var perkLine in perkLines)
        {
            //clear if first words length is less than 3
            var perkNameFixed = perkLine;
            if (perkNameFixed.Contains("Empty Socket")) item.GemId = 0;
            var perk = PerkMgr.This.Get(perkNameFixed);
            var gem = GemMgr.This.Get(perkNameFixed.FixGemNameText());
            if (gem != null)
            {
                item.GemId = gem.Id;
                continue;
            }

            if (perk != null) perkNoList.Add(perk.Id);
        }

        if (perkNoList.Count == 0) errorList.Add(ErrCode.OcrPerkNotFound.ToMessage());
        try
        {
            item.Perk1 = perkNoList[0];
            item.Perk2 = perkNoList[1];
            item.Perk3 = perkNoList[2];
        }
        catch
        {
            // ignored
        }

        var perkCount = perkNoList.Count;

        #endregion

        #region READ - GEARSCORE

        if (perkCount == 3)
        {
            item.GearScore = 600;
        }
        else
        {
            var secondPageFirstLine = secondPageLines.First().RemoveSpecialCharacters().RemoveWhitespace();
            var gearScoreParseResult = int.TryParse(secondPageFirstLine, out var itemGearScore);
            if (!gearScoreParseResult)
            {
                errorList.Add(ErrCode.OcrGearScoreParseError.ToMessage());
            }
            else
            {
                if (itemGearScore > ConstMgr.MaxGearScore)
                    errorList.Add(ErrCode.OcrGearScoreTooHighError.ToMessage());
                else if (itemGearScore < ConstMgr.MinGearScore)
                    errorList.Add(ErrCode.OcrGearScoreTooLowError.ToMessage());
            }
        }

        #endregion

        #region READ - ATTRIBUTE

        var attributeList = new List<int>();
        var attributeLines = secondPageLines
            .Where(x => x.Contains('+') && !x.Contains(":"))
            .Select(x => x.RemoveLineEndings()
                .RemoveSpecialCharacters()
                .Trim()
                .SeparateWordsAndRemoveIfFirstWordIsShort(2)
                .RemoveNumbers()
                .Trim())
            .ToList();
        foreach (var attr in attributeLines)
        {
            var isValidAttributeName = Enum.TryParse(attr, true, out AttributeType attributeType);
            if (!isValidAttributeName)
                continue;
            attributeList.Add((int)attributeType);
        }

        if (attributeList.Count == 0) errorList.Add(ErrCode.OcrAttributeNotFound.ToMessage());
        if (attributeList.Count > 2) errorList.Add(ErrCode.OcrAttributeCountInvalid.ToMessage());

        try
        {
            item.Attribute1 = attributeList.ElementAt(0);
            //item.AttributeValue_1= attributeDic.ElementAt(0).Value;
            item.Attribute2 = attributeList.ElementAt(1);
            //item.AttributeValue_2 = attributeDic.ElementAt(1).Value;
        }
        catch
        {
            // ignored
        }

        #endregion

        var ocrResult = new OcrReadResult();
        ocrResult.Item = item;
        ocrResult.Errors = errorList;
        ocrResult.FullImageBytes = _itemImageData.FullImageBytes;
        ocrResult.IconBytes = _itemImageData.IconImageBytes;
        var obj = new
        {
            First = firstPageText,
            Rest = restPageText
        };
        ocrResult.OcrTextResult = obj.ToJsonString();
        return ocrResult;
    }
}