using EasMe;
using EasMe.Extensions;
using EasMe.Result;
using IronOcr;
using NewWorld.BiSMarket.Core.Constants;
using NewWorld.BiSMarket.Core.Models;
using NewWorld.BiSMarket.Core.Properties;
using System.Drawing;
using System.Linq;
using Tesseract;
using static IronOcr.OcrResult;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace NewWorld.BiSMarket.Core;
/// <summary>
/// Resizing and parsing full image to separate images, each image will be read separately by OCR
/// </summary>
public class ItemImageOcrV3
{

    /*
    Epic
     input.AdaptiveThreshold(1.8F);
        input.DeNoise();
     
     */


    private OcrInput _ocrInput;
    private readonly ItemImageData _itemImageData;

    private ItemImageOcrV3(OcrInput input, ItemImageData itemImageData)
    {
        _ocrInput = input;
        _itemImageData = itemImageData;
        //input.Contrast(1.8F);
        input.AdaptiveThreshold(1.45F);
        //input.DeNoise();
        
        //input.SelectTextColors(ConstMgr.ItemTooltipTextColorList,20);
    }


    public static ItemImageOcrV3 Create(byte[] bytes)
    {
        const int imageWidth = 455;
        var resizedBytes = CommonLib.ResizeImageWidth(bytes, imageWidth, out var imageHeight);
        var rectIcon = new Rectangle(0, 0, 130, 130);
        var rectFirstPart = new Rectangle(130, 0, imageWidth - 130, 150);//changed
        var rectRest = new Rectangle(65, 145, imageWidth - 65, imageHeight - 145);
        var ms = new MemoryStream(resizedBytes);
        var fullImage = new Bitmap(ms);
        var icon = fullImage.Clone(rectIcon, fullImage.PixelFormat);
        var firstPart = fullImage.Clone(rectFirstPart, fullImage.PixelFormat);
        var rest = fullImage.Clone(rectRest, fullImage.PixelFormat);

        using var iconStream = new MemoryStream();
        icon.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png);
        var iconBytes = iconStream.ToArray();

        using var firstPartStream = new MemoryStream();
        firstPart.Save(firstPartStream, System.Drawing.Imaging.ImageFormat.Png);
        var firstPartBytes = firstPartStream.ToArray();

        using var restStream = new MemoryStream();
        rest.Save(restStream, System.Drawing.Imaging.ImageFormat.Png);
        var restBytes = restStream.ToArray();

        var input = new OcrInput();
        //input.AddImage(iconBytes);
        input.AddImage(firstPartBytes);
        input.AddImage(restBytes);
        var itemImageData = new ItemImageData
        {
            IconImageBytes = iconBytes,
            FullImageBytes = resizedBytes,
        };
        return new ItemImageOcrV3(input, itemImageData);
    }


    public void SaveAsImages(string outputDirectory)
    {
        _ocrInput.SaveAsImages(outputDirectory);
    }
    public ResultData<OcrReadResult> Read()
    {
        var ocr = new IronTesseract
        {
            Language = ConstMgr.DefaultOcrLanguage,
            Configuration =
            {
                BlackListCharacters = ConstMgr.OcrIgnoredCharacters,
                //TesseractVariables = 
            }
        };
        var result = ocr.Read(_ocrInput);
        
        if (result.Pages.Length != 2)
        {
            return Result.Fatal(ErrCode.OcrPageCountError);
        }
        var errorList = new List<string>();
        _ = result.Pages[0]; //icon irrelevant
        var firstPartPage = result.Pages[0];
        var restPage = result.Pages[1];
        if (!ConstMgr.IsDevelopment)
        {
            //Or check if text ocr has Bind on Equip in it, this will prevent screenshots from marketplace to be listed
            if (!restPage.Text.Contains("On Equip", StringComparison.OrdinalIgnoreCase))
            {
                return Result.Warn("This item can not be traded");
            }
            
        }

        #region READ AND CLEAR PAGES AND LINES
        var firstPageText = firstPartPage.Text.RemoveSpecialCharacters(":+");
        var firstPageLines = firstPartPage.Lines.Select(x => x.Text.RemoveSpecialCharacters(":+").Trim()).ToList();
        var secondPageText = restPage.Text.RemoveSpecialCharacters(":+");
        var secondPageLines = restPage.Lines.Select(x => x.Text.RemoveSpecialCharacters(":+").Trim()).ToList();


        var firstPageLineBeforeLastLine = firstPageLines[^2];
        var firstPageLastLine = firstPageLines.Last();
        var firstPageExceptLast2Lines = firstPageLines.Take(firstPageLines.Count - 2).ToList();
        #endregion


        var item = new ItemV3();
        item.ItemName = string.Join(" ", firstPageExceptLast2Lines).RemoveLineEndings().Replace("  ", " ").Trim();

        #region READ - RARITY AND IS NAMED
        var itemRarity = firstPageLineBeforeLastLine.RemoveLineEndings().Trim();
        var isNamed = itemRarity.Contains("Named");
        if (isNamed)
        {
            itemRarity = itemRarity.Replace("Named", "").Trim();
        }
        var rarityParseResult = Enum.TryParse(typeof(RarityType), itemRarity, true, out var itemRarityEnum);
        if (!rarityParseResult)
        {
            errorList.Add(ErrCode.OcrRarityParseError.ToMessage());
            item.Rarity = (int)RarityType.Epic;
        }
        else
        {
            item.Rarity = (int)(RarityType)itemRarityEnum;
        }
        item.IsNamed = isNamed;
        #endregion

        #region READ - ITEM TYPE
        var itemType = firstPageLastLine.RemoveLineEndings().Replace(" ", "_").Trim(); //Replace for enum string matching
        var itemTypeParseResult = Enum.TryParse(typeof(ItemType), itemType, true, out var itemTypeEnum);
        if (!itemTypeParseResult)
        {
            errorList.Add(ErrCode.OcrItemTypeParseError.ToMessage());
        }
        else
        {
            item.ItemType = (int)(ItemType)itemTypeEnum;
        }
        #endregion
        
        #region READ - LEVEL REQ
        var requirementLine = secondPageLines.FirstOrDefault(x => x.Contains("Level ", StringComparison.OrdinalIgnoreCase));
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
            .Select(x => x.SeparateWordsAndRemoveIfFirstWordIsShort(3).Split(':')[0].RemoveSpecialCharacters("'").RemoveNumbers().Trim())
            .ToList();
        perkLines.RemoveAll(x => x.IsNullOrEmpty());
        foreach (var perkLine in perkLines)
        {
            //clear if first words length is less than 3
            var perkNameFixed = perkLine;
            if (perkNameFixed.Contains("Empty Socket"))
            {
                item.GemId = 0;
            }
            var perk = PerkMgr.This.GetPerk(perkNameFixed);
            var gem = GemMgr.This.Get(perkNameFixed.FixGemNameText());
            if (gem != null)
            {
                item.GemId = gem.Id;
                continue;
            }
            if (perk != null)
            {
                perkNoList.Add(perk.Id);
                continue;
            }
        }

        if (perkNoList.Count == 0)
        {
            errorList.Add(ErrCode.OcrPerkNotFound.ToMessage());
        }
        try
        {
            item.Perk_1 = perkNoList[0];
            item.Perk_2 = perkNoList[1];
            item.Perk_3 = perkNoList[2];
        }
        catch
        {
            // ignored
        }

        var perkCount = perkNoList.Count;

        #endregion

        #region READ - GEARSCORE

        var secondPageFirstLine = secondPageLines.First().RemoveSpecialCharacters().RemoveWhitespace();
        var gearScoreParseResult = int.TryParse(secondPageFirstLine, out var itemGearScore);
        if (!gearScoreParseResult)
        {
            if (perkCount == 3)
            {
                item.GearScore = 600;
            }
            else
            {
                errorList.Add(ErrCode.OcrGearScoreParseError.ToMessage());
            }
            errorList.Add(ErrCode.OcrGearScoreParseError.ToMessage());
        }
        else
        {
            if (itemGearScore > 600)
            {
                itemGearScore = 600;
            }
            else if (itemGearScore < 500)
            {
                var gsAsStr = itemGearScore.ToString();
                var changeFirstDigitTo5 = gsAsStr.Replace(gsAsStr[0], '5');
                itemGearScore = int.Parse(changeFirstDigitTo5);
            }
            item.GearScore = itemGearScore;

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
        if (attributeList.Count == 0)
        {
            errorList.Add(ErrCode.OcrAttributeNotFound.ToMessage());
        }
        if (attributeList.Count > 2)
        {
            errorList.Add(ErrCode.OcrAttributeCountInvalid.ToMessage());
        }

        try
        {
            item.Attribute_1 = attributeList.ElementAt(0);
            //item.AttributeValue_1= attributeDic.ElementAt(0).Value;
            item.Attribute_2 = attributeList.ElementAt(1);
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
        ocrResult.OcrTextResult = result.Text;
        return ocrResult;
        

    }




}