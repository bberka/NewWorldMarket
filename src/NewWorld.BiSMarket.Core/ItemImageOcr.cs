﻿using EasMe;
using EasMe.Extensions;
using EasMe.Result;
using IronOcr;
using NewWorld.BiSMarket.Core.Constants;
using NewWorld.BiSMarket.Core.Models;
using NewWorld.BiSMarket.Core.Properties;
using static IronOcr.OcrResult;

namespace NewWorld.BiSMarket.Core;

public class ItemImageOcr
{

    private OcrInput _ocrInput;
    private ItemImageOcr(OcrInput input)
    {
        _ocrInput = input;
        input.Scale(200);
        input.Contrast(1.6F);
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
            }
        };
        var result = ocr.Read(_ocrInput);
        if (!ConstMgr.IsDevelopment)
        {
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
        ImportPerks(ocrResult.Lines, ocrResult.Text, ref item);
        ImportAttributes(ocrResult.Lines, ref item);
        ImportGemData(ocrResult.Text, ocrResult.Lines, ref item);
        ImportItemType(ocrResult.Text, ref item);
        ImportRarity(ocrResult.Text, ref item);
        ImportLevelRequirement(ocrResult.Lines, ref item);
        ImportTier(ocrResult.Lines, ref item);
        item.IsNamed = IsNamed(ocrResult.Text);
        return item;
    }

    private static Result ImportGemData(string ocrTextResult, IEnumerable<Line> lines, ref Item item)
    {
        var isEmptySocket = ocrTextResult.Contains("Empty Socket", StringComparison.OrdinalIgnoreCase);
        if (isEmptySocket)
        {
            item.IsEmptySocket = true;
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
            var firstPart = split[0];
            var gemPerk = GemMgr.This.Get(firstPart);
            if (gemPerk is null)
            {
                var split2 = firstPart.Split(' ');
                var lastTwoMerge = split2[^2] + " " + split2[^1];
                gemPerk = GemMgr.This.Get(lastTwoMerge);
                if (gemPerk is null)
                    continue;
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
        if (listAttributes.Count == 0) Result.Error("No attribute is found");
        item.Attributes = string.Join(",", listAttributes);
        return true;
    }
    private static Result ImportPerks(IEnumerable<Line> lines, string ocrTextResult, ref Item item)
    {
        //var perkLines = lines
        //    .Where(x => x.Text.Contains(':') 
        //                //&& !x.Text.Contains("Requirement: Level") 
        //                //&& !x.Text.Contains("I:", StringComparison.OrdinalIgnoreCase) 
        //                //&& !x.Text.Contains("II:", StringComparison.OrdinalIgnoreCase) 
        //                //&& !x.Text.Contains("III:", StringComparison.OrdinalIgnoreCase) 
        //                //&& !x.Text.Contains("IV:",StringComparison.OrdinalIgnoreCase) 
        //                //&& !x.Text.Contains("V:", StringComparison.OrdinalIgnoreCase) 
        //                )
        //    .ToList();
        //foreach (var perk in perkLines)
        //{
        //    var split = perk.Text.Split(':');
        //    var perkOrGemName = split[0].Trim();
        //    var splitName = perkOrGemName.Split(' ').ToList();
        //    //splitName.RemoveAt(0);
        //    var name = string.Join(' ', splitName);
        //    if(name.IsNullOrEmpty() || name.Length < 2) continue;
        //    if (!IsValidPerk(name))
        //        continue;
        //    list.Add(name);
        //}

        //var resourcePerkList = Resource.PerkListV2.Split(Environment.NewLine);
        var list = new List<int>();
        foreach (var perk in PerkMgr.This.Perks)
        {
            if (ocrTextResult.Contains(perk.EnglishName + ":", StringComparison.OrdinalIgnoreCase))
                list.Add(perk.Id);
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
        var gradeTypeList = Enum.GetNames(typeof(GradeType)).ToList();
        var gradeTypeMatchInResultText = gradeTypeList
            .FirstOrDefault(x => ocrResultText.Contains(x, StringComparison.OrdinalIgnoreCase));
        var parseToEnum = Enum.TryParse(gradeTypeMatchInResultText, out GradeType gradeType);
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
            item.LevelRequirement = 0;
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
            item.Tier = 0;
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