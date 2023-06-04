using System.Text;
using EasMe.Extensions;
using NewWorld.BiSMarket.Core.Models;
using NewWorld.BiSMarket.Core.Properties;

namespace NewWorld.BiSMarket.Core;

public class PerkMgr
{
    private static PerkMgr? Instance;
    private readonly List<Perk> _list;

    private PerkMgr()
    {
        var perkList = Resource.PerkListV2.Split(Environment.NewLine);
        _list = new List<Perk>();
        for (var i = 0; i < perkList.Length; i++)
        {
            var perk = perkList[i];
            var perkModel = new Perk();
            perkModel.Id = i;
            perkModel.EnglishName = perk;
            _list.Add(perkModel);
        }
    }

    public static PerkMgr This
    {
        get
        {
            Instance ??= new PerkMgr();
            return Instance;
        }
    }

    public IReadOnlyCollection<Perk> Perks => _list;

    public Perk? GetPerk(int id)
    {
        return _list.FirstOrDefault(x => x.Id == id);
    }

    public bool IsValid(string name)
    {
        var nameTrim = name.Trim();
        return _list.Any(x => x.EnglishName.Equals(nameTrim, StringComparison.OrdinalIgnoreCase));
    }

    public bool IsValid(int id)
    {
        return _list.Any(x => x.Id == id);
    }

    public Perk? GetPerk(string name)
    {
        var nameTrim = name.Trim();
        return _list.FirstOrDefault(x => x.EnglishName.Equals(nameTrim, StringComparison.OrdinalIgnoreCase));
    }

    public string ParsePerkFormattedTextHtmlRaw(string text)
    {
        if (text.IsNullOrEmpty()) return "Could not be read";

        var split = text.Split(",");
        var sb = new StringBuilder();
        foreach (var s in split)
        {
            var perkId = int.Parse(s);
            if (perkId < 0) continue;
            var perk = GetPerk(perkId);
            if (perk == null) continue;
            sb.Append($"{perk.EnglishName}");
            sb.Append("<br/>");
        }

        return sb.ToString();
    }

    public string ParsePerkFormattedText(string text)
    {
        if (text.IsNullOrEmpty()) return "Could not be read";
        var split = text.Split(",");
        var sb = new StringBuilder();
        foreach (var s in split)
        {
            var perkId = int.Parse(s);
            if (perkId < 0) continue;
            var perk = GetPerk(perkId);
            if (perk == null) continue;
            sb.Append($"{perk.EnglishName}");
            sb.Append(Environment.NewLine);
        }

        return sb.ToString();
    }
}