using System.Text;
using NewWorldMarket.Core.Models;
using NewWorldMarket.Core.Properties;

namespace NewWorldMarket.Core.Tools;

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

    public Perk? Get(int id)
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

    public Perk? Get(string name)
    {
        var nameTrim = name.Trim();
        return _list.FirstOrDefault(x => x.EnglishName.Equals(nameTrim, StringComparison.OrdinalIgnoreCase));
    }

    //public string ParsePerkFormattedTextHtmlRaw(string text)
    //{
    //    if (text.IsNullOrEmpty()) return "Could not be read";

    //    var split = text.Split(",");
    //    var sb = new StringBuilder();
    //    foreach (var s in split)
    //    {
    //        var perkId = int.Parse(s);
    //        if (perkId < 0) continue;
    //        var perk = Get(perkId);
    //        if (perk == null) continue;
    //        sb.Append($"{perk.EnglishName}");
    //        sb.Append("<br/>");
    //    }

    //    return sb.ToString();
    //}

    //public string ParsePerkFormattedText(string text)
    //{
    //    if (text.IsNullOrEmpty()) return "Could not be read";
    //    var split = text.Split(",");
    //    var sb = new StringBuilder();
    //    foreach (var s in split)
    //    {
    //        var perkParseResult = int.TryParse(s,out var parsed);
    //        if (!perkParseResult) continue;
    //        if (parsed < 0) continue;
    //        var perk = Get(parsed);
    //        if (perk == null) continue;
    //        sb.Append($"{perk.EnglishName}");
    //        sb.Append(Environment.NewLine);
    //    }

    //    var result = sb.ToString();
    //    if (result.IsNullOrEmpty()) return "Could not be read";
    //    return result;

    //}
    public string GetAsViewText(int id, int id2,int id3,string separator)
    {
        if (id == id2) id2 = -1;
        if (id == id3) id3 = -1;
        if (id2 == id3) id3 = -1;
        if (id < 0 && id2 < 0 && id3 < 0) return "Could not be read";
        var sb = new StringBuilder();
        if (id >= 0)
        {
            var perk = Get(id);
            if (perk != null)
            {
                sb.Append($"{perk.EnglishName}");
                sb.Append('<');
            }
        }
        if (id2 >= 0)
        {
            var perk = Get(id2);
            if (perk != null)
            {
                sb.Append($"{perk.EnglishName}");
                sb.Append('<');

            }
        }

        if (id3 >= 0)
        {
            var perk = Get(id3);
            if (perk != null)
            {
                sb.Append($"{perk.EnglishName}");
                sb.Append('<');
            }
        }
        return sb.ToString().Trim('<').Replace("<", separator);
    }
}