using EasMe;
using EasMe.Extensions;
using EasMe.Result;
using IronOcr;
using NewWorld.BiSMarket.Core.Constants;
using NewWorld.BiSMarket.Core.Models;
using NewWorld.BiSMarket.Core.Properties;
using static IronOcr.OcrResult;

namespace NewWorld.BiSMarket.Core;

#if OCR_v1



[Obsolete]
public class ItemImageOcr
{
    /*
    Epic
     input.AdaptiveThreshold(1.8F);
        input.DeNoise();
     
     */

    private OcrInput _ocrInput;
    private ItemImageOcr(OcrInput input)
    {
        _ocrInput = input;
        //input.Contrast(1.4F);
        input.AdaptiveThreshold(1.65F);
        //input.DeNoise();

        input.SaveAsImages(@"C:\Users\kkass\OneDrive\Masaüstü\test.png");
        //input.SelectTextColors(ConstMgr.ItemTooltipTextColorList,20);
    }

    public static ItemImageOcr Create(string filePath)
    {
        var input = new OcrInput();
        input.AddImage(filePath);
        return new ItemImageOcr(input);
    }
    public static ItemImageOcr CreateByBase64String(string base64text)
    {
        var input = new OcrInput();
        input.AddImage(base64text);
        return new ItemImageOcr(input);
    }

    public static ItemImageOcr Create(byte[] bytes)
    {
        var input = new OcrInput();
        input.AddImage(bytes);
        return new ItemImageOcr(input);
    }
    public static ItemImageOcr CreateByUrl(string url)
    {
        var input = new OcrInput();
        input.AddImage(url);
        return new ItemImageOcr(input);
    }

    public void SaveAsImages(string path)
    {
        _ocrInput.SaveAsImages(path);
    }
    public ResultData<Item> Read(out string textResult)
    {
        textResult = string.Empty;
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
        if (!ConstMgr.IsDevelopment)
        {
            //Or check if text ocr has Bind on Equip in it, this will prevent screenshots from marketplace to be listed
            if (result.Text.Contains("Bind On Pickup", StringComparison.OrdinalIgnoreCase))
                return Result.Error("This item can not be traded because it is bind on pickup");
            if (result.Text.Contains("Bound To Player", StringComparison.OrdinalIgnoreCase))
                return Result.Error("This item can not be traded because it is bound to player");
        }
        var item = FromOcrResult(result);
        return item;
    }
    public ResultData<Item> Read()
    {
        return Read(out _);
    }

    private static Item FromOcrResult(OcrResult ocrResult)
    {
        var item = new Item();
        var results = new List<Result>()
        {
            ImportPerks(ocrResult.Lines, ocrResult.Text, ref item),
            ImportAttributes(ocrResult.Lines, ref item),
            ImportGemData(ocrResult.Text, ocrResult.Lines, ref item),
            ImportItemType(ocrResult.Text, ref item),
            ImportRarity(ocrResult.Text, ref item),
            ImportLevelRequirement(ocrResult.Lines, ref item),
            ImportTier(ocrResult.Lines, ref item),
            ImportGearScore(ocrResult.Lines,ref item)
        };
        var errors = results.Where(x => x.IsFailure).ToList();

        item.IsNamed = IsNamed(ocrResult.Text);
        return item;
    }

    private static Result ImportGearScore(IEnumerable<Line> lines, ref Item item)
    {
        var gsLine = lines.FirstOrDefault(x => x.Text.StartsWith("®", StringComparison.OrdinalIgnoreCase));
        if (gsLine is null)
            return Result.Warn("Gear score not found");
        var gsText = gsLine.Text.Trim().Replace("®","");
        var tryParse = int.TryParse(gsText, out var gs);
        if (!tryParse)
            return Result.Warn("Gear score not found");
        item.GearScore = gs;
        return true;
    }
    private static Result ImportGemData(string ocrTextResult, IEnumerable<Line> lines, ref Item item)
    {
        var isEmptySocket = ocrTextResult.Contains("Empty Socket", StringComparison.OrdinalIgnoreCase);
        if (isEmptySocket)
        {
            item.GemId = 0;
            return true;
        }
        var gemLineList = lines.Where(x => x.Text.Contains(":", StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (gemLineList.Count == 0)
        {
            return Result.Warn("No gem found");
        }

        foreach (var gemLine in gemLineList)
        {
            var split = gemLine.Text.Split(':');
            if(split.Length < 2)
                continue;
            var firstPart = split[0].RemoveSpecialCharacters().Trim();
            var splitSpace = firstPart.Split(' ');
            if (splitSpace.Length < 2)
                continue;
            var gemPerk = GemMgr.This.Get(splitSpace[0]);
            if (gemPerk is null)
            {
                continue;
                //var split2 = firstPart.Split(' ');
                //var lastTwoMerge = split2[^2] + " " + split2[^1];
                //gemPerk = GemMgr.This.Get(lastTwoMerge);
                //if (gemPerk is null)
                //    continue;
            }
            item.GemId = gemPerk.Id;
            break;
        }

        return true;
    }
    private static Result ImportAttributes(IEnumerable<Line> lines, ref Item item)
    {
        var listAttributes = new List<string>();
        var attributeLinesNotProcessed = lines
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
            var value = int.Parse(split[0]);
            listAttributes.Add((int)attributeType + ":" + value);
        }
        if (listAttributes.Count == 0) return Result.Error("No attribute is found");
        item.Attributes = string.Join(",", listAttributes);
        return true;
    }
    private static Result ImportPerks(IEnumerable<Line> lines, string ocrTextResult, ref Item item)
    {
        var list = new List<int>();
        var perkLines = lines.Where(x => x.Text.Contains(":"))
            .ToList();
        foreach (var perkLine in perkLines)
        {
            var first = perkLine.Text.Split(':')[0];
            var perkName = first.RemoveSpecialCharacters().Trim();
            //remove single character if after space 
            var split = perkName.Split(' ');
            if (split.Length > 1)
            {
                var first2 = split[0];
                if (first2.Length == 1)
                {
                    perkName = perkName.Replace(first2, "");
                }
            }
            var isValidPerk = IsValidPerk(perkName);
            if (!isValidPerk)
                continue;
            var perk = PerkMgr.This.GetPerk(perkName);
            if (perk is null)
                continue;
            list.Add(perk.Id);
        }
       

        if (list.Count == 0)
        {
            return Result.Error("No perk is found");
        }
        item.Perks = string.Join(",", list);
        return true;
    }

    private static bool IsValidPerk(string perkName)
    {
        var resourcePerkList = Resource.PerkListV2.Split(Environment.NewLine);
        var isValidPerkName = resourcePerkList.Any(x => x.Equals(perkName, StringComparison.OrdinalIgnoreCase));
        return isValidPerkName;
    }
    private static Result ImportItemType(string ocrResultText, ref Item item)
    {
        var itemTypeList = Enum.GetNames(typeof(ItemType)).ToList();
        var itemTypeName = itemTypeList
            .FirstOrDefault(x => ocrResultText.Contains(x.Replace("_", " "), StringComparison.Ordinal));
        if (itemTypeName == null)
            return Result.Error("Item type text is not found");
        var parse = Enum.TryParse(itemTypeName, out ItemType itemType);
        if (!parse)
            return Result.Error("Item type is not found");
        item.ItemType = (byte)itemType;
        return true;
    }

    private static Result ImportRarity(string ocrResultText, ref Item item)
    {
        var gradeTypeList = Enum.GetNames(typeof(RarityType)).ToList();
        var gradeTypeMatchInResultText = gradeTypeList
            .FirstOrDefault(x => ocrResultText.Contains(x, StringComparison.OrdinalIgnoreCase));
        var parseToEnum = Enum.TryParse(gradeTypeMatchInResultText, out RarityType gradeType);
        if (!parseToEnum)
            return Result.Error("Rarity type is not found");
        item.Rarity = (byte)gradeType;
        return true;
    }
    private static Result ImportLevelRequirement(IEnumerable<Line> lines, ref Item item)
    {
        var line = lines.FirstOrDefault(x => x.Text.Contains("Requirement: Level ", StringComparison.OrdinalIgnoreCase));
        if (line == null)
        {
            return Result.Warn("Requirement level not found");
        }
        var textAfterLevel = line.Text.Split("Level ")[1];
        item.LevelRequirement = byte.Parse(textAfterLevel);
        return true;
    }
    private static bool IsNamed(string content)
    {
        return content.Contains("Named", StringComparison.OrdinalIgnoreCase);
    }
    private static Result ImportTier(IEnumerable<Line> lines, ref Item item)
    {
        var line = lines.FirstOrDefault(x => x.Text.Contains("Tier ", StringComparison.OrdinalIgnoreCase));
        if (line == null)
        {
            return Result.Warn("Tier text not found");
        }
        var textAfterTier = line.Text.Split("Tier ")[1];
        var parseEnum = Enum.TryParse(textAfterTier, out TierType tierType);
        if (!parseEnum)
            return Result.Error("Tier type not found");
        item.Tier = (byte)tierType;
        return true;
    }


}

#endif