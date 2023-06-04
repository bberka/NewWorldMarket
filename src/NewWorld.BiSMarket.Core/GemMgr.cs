using NewWorld.BiSMarket.Core.Models;
using NewWorld.BiSMarket.Core.Properties;

namespace NewWorld.BiSMarket.Core;

public class GemMgr
{
    private static GemMgr? Instance;
    private readonly List<Perk> _list;

    private GemMgr()
    {
        var perkList = Resource.GemList.Split(Environment.NewLine);
        var temp = new List<Perk>();
        for (var i = 0; i < perkList.Length; i++)
        {
            var perk = perkList[i];
            var perkModel = new Perk();
            perkModel.Id = i;
            perkModel.EnglishName = perk;
            temp.Add(perkModel);
        }

        _list = temp.DistinctBy(x => x).ToList();
    }

    public static GemMgr This
    {
        get
        {
            Instance ??= new GemMgr();
            return Instance;
        }
    }

    public IReadOnlyCollection<Perk> Gems => _list;

    public bool IsValidGem(string name)
    {
        var trimName = name.Trim();
        return _list.Any(x => x.EnglishName.Contains(trimName, StringComparison.OrdinalIgnoreCase));
    }

    public Perk? Get(string name)
    {
        var trimName = name.Trim();
        return _list.FirstOrDefault(x => x.EnglishName.StartsWith(trimName, StringComparison.OrdinalIgnoreCase));
    }

    public Perk? Get(int id)
    {
        if (id < 0) return null;
        return _list.First(x => x.Id == id);
    }
}