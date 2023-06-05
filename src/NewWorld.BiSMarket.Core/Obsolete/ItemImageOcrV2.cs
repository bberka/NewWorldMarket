namespace NewWorldMarket.Core.Obsolete;

#if OCR_v2
/// <summary>
/// Resizing and parsing full image to separate images, each image will be read separately by OCR
/// </summary>
///
[Obsolete]
public class ItemImageOcrV2
{

    /*
    Epic
     input.AdaptiveThreshold(1.8F);
        input.DeNoise();
     
     */


    private OcrInput _ocrInput;
    private readonly ItemImageData _itemImageData;

    private ItemImageOcrV2(OcrInput input, ItemImageData itemImageData)
    {
        _ocrInput = input;
        _itemImageData = itemImageData;
        //input.Contrast(1.8F);
        input.AdaptiveThreshold(1.45F);
        //input.DeNoise();

        input.SaveAsImages(@"C:\Users\kkass\OneDrive\Masaüstü\outputOCR\");
        //input.SelectTextColors(ConstMgr.ItemTooltipTextColorList,20);
    }


    public static ItemImageOcrV2 CreateForSecondTry(byte[] bytes)
    {
        const int imageWidth = 455;
        var resizedBytes = ResizeImageWidth(bytes, imageWidth, out var imageHeight);
        var rectIcon = new Rectangle(0, 0, 130, 130);
        var rectName = new Rectangle(130, 0, imageWidth - 130, 100);//changed
        var rectRarity = new Rectangle(130, 95, imageWidth - 130, 32);
        var rectTypeName = new Rectangle(130, 122, imageWidth - 130, 32);
        var rectGearScore = new Rectangle(67, 160, 100, 80);
        var rectRest = new Rectangle(0, 230, imageWidth, imageHeight - 230);
        var ms = new MemoryStream(resizedBytes);
        var fullImage = new Bitmap(ms);
        var icon = fullImage.Clone(rectIcon, fullImage.PixelFormat);
        var name = fullImage.Clone(rectName, fullImage.PixelFormat);
        var rarity = fullImage.Clone(rectRarity, fullImage.PixelFormat);
        var typeName = fullImage.Clone(rectTypeName, fullImage.PixelFormat);
        var gearScore = fullImage.Clone(rectGearScore, fullImage.PixelFormat);
        var rest = fullImage.Clone(rectRest, fullImage.PixelFormat);

        using var iconStream = new MemoryStream();
        icon.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png);
        var iconBytes = iconStream.ToArray();

        using var nameStream = new MemoryStream();
        name.Save(nameStream, System.Drawing.Imaging.ImageFormat.Png);
        var nameBytes = nameStream.ToArray();

        using var rarityStream = new MemoryStream();
        rarity.Save(rarityStream, System.Drawing.Imaging.ImageFormat.Png);
        var rarityBytes = rarityStream.ToArray();

        using var typeNameStream = new MemoryStream();
        typeName.Save(typeNameStream, System.Drawing.Imaging.ImageFormat.Png);
        var typeNameBytes = typeNameStream.ToArray();

        using var gearScoreStream = new MemoryStream();
        gearScore.Save(gearScoreStream, System.Drawing.Imaging.ImageFormat.Png);
        var gearScoreBytes = gearScoreStream.ToArray();

        using var restStream = new MemoryStream();
        rest.Save(restStream, System.Drawing.Imaging.ImageFormat.Png);
        var restBytes = restStream.ToArray();

        var input = new OcrInput();
        input.AddImage(iconBytes);
        input.AddImage(nameBytes);
        input.AddImage(rarityBytes);
        input.AddImage(typeNameBytes);
        input.AddImage(gearScoreBytes);
        input.AddImage(restBytes);
        var itemImageData = new ItemImageData
        {
            IconImageBytes = iconBytes,
            FullImageBytes = resizedBytes,

        };
        return new ItemImageOcrV2(input, itemImageData);
    }

    //Works with 2 line name
    public static ItemImageOcrV2 Create(byte[] bytes)
    {
        const int imageWidth = 455;
        var resizedBytes = ResizeImageWidth(bytes, imageWidth, out var imageHeight);
        var rectIcon = new Rectangle(0, 0, 130, 130);
        var rectName = new Rectangle(130, 0, imageWidth - 130, 80);
        var rectRarity = new Rectangle(130, 65, imageWidth - 130, 35);
        var rectTypeName = new Rectangle(130, 95, imageWidth - 130, 35);
        var rectGearScore = new Rectangle(63, 130, 100, 80);
        var rectRest = new Rectangle(0, 200, imageWidth, imageHeight - 200);
        var ms = new MemoryStream(resizedBytes);
        var fullImage = new Bitmap(ms);
        var icon = fullImage.Clone(rectIcon, fullImage.PixelFormat);
        var name = fullImage.Clone(rectName, fullImage.PixelFormat);
        var rarity = fullImage.Clone(rectRarity, fullImage.PixelFormat);
        var typeName = fullImage.Clone(rectTypeName, fullImage.PixelFormat);
        var gearScore = fullImage.Clone(rectGearScore, fullImage.PixelFormat);
        var rest = fullImage.Clone(rectRest, fullImage.PixelFormat);

        using var iconStream = new MemoryStream();
        icon.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png);
        var iconBytes = iconStream.ToArray();

        using var nameStream = new MemoryStream();
        name.Save(nameStream, System.Drawing.Imaging.ImageFormat.Png);
        var nameBytes = nameStream.ToArray();

        using var rarityStream = new MemoryStream();
        rarity.Save(rarityStream, System.Drawing.Imaging.ImageFormat.Png);
        var rarityBytes = rarityStream.ToArray();

        using var typeNameStream = new MemoryStream();
        typeName.Save(typeNameStream, System.Drawing.Imaging.ImageFormat.Png);
        var typeNameBytes = typeNameStream.ToArray();

        using var gearScoreStream = new MemoryStream();
        gearScore.Save(gearScoreStream, System.Drawing.Imaging.ImageFormat.Png);
        var gearScoreBytes = gearScoreStream.ToArray();

        using var restStream = new MemoryStream();
        rest.Save(restStream, System.Drawing.Imaging.ImageFormat.Png);
        var restBytes = restStream.ToArray();

        var input = new OcrInput();
        input.AddImage(iconBytes);
        input.AddImage(nameBytes);
        input.AddImage(rarityBytes);
        input.AddImage(typeNameBytes);
        input.AddImage(gearScoreBytes);
        input.AddImage(restBytes);
        var itemImageData = new ItemImageData
        {
            IconImageBytes = iconBytes,
            FullImageBytes = resizedBytes,

        };
        return new ItemImageOcrV2(input,itemImageData);
    }


    private static byte[] ResizeImageWidth(byte[] imageBytes, int newWidth,out int imageHeight)
    {
        // Create a MemoryStream from the image byte array
        using MemoryStream imageStream = new MemoryStream(imageBytes);
        // Create an Image object from the MemoryStream
        using Image originalImage = Image.FromStream(imageStream);
        // Get the original width and height of the image
        int width = originalImage.Width;
        var height = originalImage.Height;

        // Calculate the aspect ratio of the image
        double aspectRatio = (double)width / height;

        // Calculate the new height based on the new width and aspect ratio
        imageHeight = (int)(newWidth / aspectRatio);

        // Create a new Bitmap object with the new dimensions
        using Bitmap resizedImage = new Bitmap(newWidth, imageHeight);
        // Create a Graphics object from the resized image
        using Graphics graphics = Graphics.FromImage(resizedImage);
        // Set the interpolation mode to high quality bicubic
        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        // Draw the original image onto the resized image
        graphics.DrawImage(originalImage, 0, 0, newWidth, imageHeight);

        // Create a MemoryStream to store the resized image
        using MemoryStream resizedImageStream = new MemoryStream();
        // Save the resized image to the MemoryStream in JPEG format
        resizedImage.Save(resizedImageStream, System.Drawing.Imaging.ImageFormat.Jpeg);

        // Get the resized image data as a byte array
        byte[] resizedImageBytes = resizedImageStream.ToArray();

        return resizedImageBytes;
    }
    public void SaveAsImages(string path)
    {
        _ocrInput.SaveAsImages(path);
    }
    public ItemOcrResult Read()
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
       
        if (result.Pages.Length != 6)
        {
            return ItemOcrResult.Fail(ErrCode.OcrPageCountError.ToMessage());
        }
        var errorList = new List<string>();
        _ = result.Pages[0]; //icon irrelevant
        var name = result.Pages[1];
        var rarity = result.Pages[2];
        var typeName = result.Pages[3];
        var gearScore = result.Pages[4];
        var rest = result.Pages[5];
        if (!ConstMgr.IsDevelopment)
        {
            //Or check if text ocr has Bind on Equip in it, this will prevent screenshots from marketplace to be listed
            if (rest.Text.Contains("Bind On Pickup", StringComparison.OrdinalIgnoreCase))
            {
                return ItemOcrResult.Fail("This item can not be traded because it is bind on pickup");
            }
            if (rest.Text.Contains("Bound To Player", StringComparison.OrdinalIgnoreCase))
            {
                return ItemOcrResult.Fail("This item can not be traded because it is bound to player");
            }
        }
        var item = new Item();
        //FULL NAME READ
        var fixedFullItemName = name.Text.RemoveSpecialCharacters().ReplaceLineEndings(" ");
        item.ItemName = fixedFullItemName;
        //TYPE NAME READ
        var fixedTypeName = typeName.Text.RemoveSpecialCharacters().Replace(" ","_");
        var typeNameParseResult = Enum.TryParse(fixedTypeName, out ItemType itemType);
        if (!typeNameParseResult)
        {
            errorList.Add(ErrCode.OcrItemTypeParseError.ToMessage());
        }
        else
        {
            item.ItemType = (int)itemType;
        }

        
        
        //ATTRIBUTE READ
        var listAttributes = new List<string>();
        var attributeLinesNotProcessed = rest.Lines
            .Where(x => x.Text.Contains('+'))
            .ToList();
        foreach (var attr in attributeLinesNotProcessed)
        {
            var textAfterPlus = attr.Text.Split('+')[1];
            var split = textAfterPlus.Split(' ');
            var attribute = split[1];
            var isValidAttributeName = Enum.TryParse(attribute, true, out AttributeType attributeType);
            if (!isValidAttributeName)
                continue;
            var parseResult = int.TryParse(split[0], out int value);
            if (!parseResult)
            {
                errorList.Add(ErrCode.OcrAttributeParseError.ToMessage());
                continue;
            }
            listAttributes.Add((int)attributeType + ":" + value);
        }
        if (listAttributes.Count == 0)
        {
            errorList.Add(ErrCode.OcrAttributeNotFound.ToMessage());
        }
        else if (listAttributes.Count > 2)
        {
            errorList.Add(ErrCode.OcrAttributeCountInvalid.ToMessage());
        }
        else
        {
            item.Attributes = string.Join(",", listAttributes);
        }
        //LEVEL REQUIREMENT READ
        var levelReqLine =
 rest.Lines.FirstOrDefault(x => x.Text.Contains("Requirement: Level ", StringComparison.OrdinalIgnoreCase));
        if (levelReqLine == null)
        {
            errorList.Add(ErrCode.OcrLevelRequirementNotFound.ToMessage());
        }
        else
        {
            var textAfterLevel = levelReqLine.Text.Split("Level ")[1];
            var parseResult = byte.TryParse(textAfterLevel, out byte levelReq);
            if (!parseResult)
            {
                errorList.Add(ErrCode.OcrLevelRequirementParseError.ToMessage());
            }
            else
            {
                item.LevelRequirement = levelReq;
            }
        }
        //TIER READ
        var tierLine = rest.Lines.FirstOrDefault(x => x.Text.Contains("Tier ", StringComparison.OrdinalIgnoreCase));
        if (tierLine == null)
        {
            errorList.Add(ErrCode.OcrTierNotFound.ToMessage());
        }
        else
        {
            var textAfterTier = tierLine.Text.Split("Tier ")[1];
            var parseEnum = Enum.TryParse(textAfterTier, out TierType tierType);
            if (!parseEnum)
            {
                errorList.Add(ErrCode.OcrTierParseError.ToMessage());
            }
            else
            {
                item.Tier = (byte)tierType;
            }
        }
        //GEM READ
        var isEmptySocket = rest.Text.Contains("Empty Socket", StringComparison.OrdinalIgnoreCase);
        if (isEmptySocket)
        {
            item.GemId = 0;
        }
        else
        {
            var gemLineList = rest.Lines.Where(x => x.Text.Contains(":"))
                .ToList();
            if (gemLineList.Count == 0)
            {
                errorList.Add(ErrCode.OcrGemNotFound.ToMessage());
            }
            else
            {
                foreach (var gemLine in gemLineList)
                {
                    var split =
 gemLine.Text.Split(':'); // Get first part which has the name of the gem and remove special characters
                    if (split.Length < 2)
                        continue;
                    var firstPart = split[0].RemoveSpecialCharacters().Trim();
                    var splitSpace = firstPart.Split(' '); //To ignore search for gem level (II - IV etc.)
                    if (splitSpace.Length < 2)
                        continue;
                    var gemPerk = GemMgr.This.Get(splitSpace[0]);
                    if (gemPerk is null)
                    {
                        continue;
                    }
                    item.GemId = gemPerk.Id;
                    break;
                }
            }

           
        }
        //PERK READ
        var perkNoList = new List<int>();
        var perkLines = rest.Lines.Where(x => x.Text.Contains(":"))
            .ToList();
        foreach (var perkLine in perkLines)
        {
            var split = perkLine.Text.Split(':');
            if (split.Length < 2)
                continue;
            var first = split[0];
            var perkName = first.RemoveSpecialCharacters("'").RemoveNumbers().Trim();
            if(perkName.IsNullOrEmpty()) continue;
            //var perkName = perkLine.Text.RemoveSpecialCharacters("'").RemoveNumbers().Trim();
            //remove single character if after space 
            var split2 = perkName.Split(' ');
            if (split2.Length < 2)
                continue;
            var first2 = split2[0];
            if (first2.Length < 3)
            {
                perkName = perkName.Replace(first2, "");
            }
           
            //this part can be removed if more accurate OCR read
            var perk = PerkMgr.This.GetPerk(perkName);
            if (perk is null)
                continue; //not a valid perk
            perkNoList.Add(perk.Id);
        }

        if (perkNoList.Count == 0)
        {
            errorList.Add(ErrCode.OcrPerkNotFound.ToMessage());
        }
        else
        {
            item.Perks = string.Join(",", perkNoList);
        }
        //GS READ
        var fixedGearScore = gearScore.Text.RemoveSpecialCharacters().RemoveWhitespace().RemoveLineEndings();
        var gearScoreParseResult = int.TryParse(fixedGearScore, out int itemGearScore);
        if (!gearScoreParseResult)
        {
            if (perkNoList.Count == 3)
            {
                item.GearScore = 600;
            }
            else
            {
                errorList.Add(ErrCode.OcrGearScoreParseError.ToMessage());
            }
        }
        else
        {
            item.GearScore = itemGearScore;
        }
        //RARITY AND NAMED READ
        var fixedRarity = rarity.Text.RemoveSpecialCharacters();
        item.IsNamed = fixedRarity.Contains("Named");
        fixedRarity = fixedRarity.Replace("Named", "").RemoveWhitespace().RemoveLineEndings();
        var rarityParseResult = Enum.TryParse(fixedRarity, out RarityType itemRarity);
        if (!rarityParseResult)
        {
            if (perkNoList.Count == 3)
            {
                item.Rarity = (int)RarityType.Legendary;
            }
            else
            {
                errorList.Add(ErrCode.OcrRarityParseError.ToMessage());
            }
        }
        else
        {
            item.Rarity = (int)itemRarity;
        }

        var pageDictionary = result.Pages.ToDictionary(x => x.PageNumber, x => x.Text);
        pageDictionary.Remove(0);
        if (errorList.Count == 0)
        {
            return ItemOcrResult.Success(
                _itemImageData,
                item,
                pageDictionary);
        }

        if (errorList.Count < 3)
        {
            return ItemOcrResult.SuccessWithErrors(
                _itemImageData, 
                item,
                pageDictionary, 
                errorList.ToArray());
        }
        return ItemOcrResult.Fail(
            //_itemImageData, 
            //item,
            //pageDictionary, 
            errorList.ToArray());
        

    }




}
#endif