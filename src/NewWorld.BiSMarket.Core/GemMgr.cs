using NewWorld.BiSMarket.Core.Models;
using NewWorld.BiSMarket.Core.Properties;

namespace NewWorld.BiSMarket.Core;

public class GemMgr
{

    private GemMgr()
    {
        var perkList = Resource.GemList.Split(Environment.NewLine);//Todo: import gem list txt
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
	public static GemMgr This
	{
		get
		{
			Instance ??= new();
			return Instance;
		}
	}
	private static GemMgr? Instance;
    private List<Perk> _list;
    public IReadOnlyCollection<Perk> Gems => _list;

    public bool IsValidGem(string name)
    {
        var trimName = name.Trim();
        return _list.Any(x => x.EnglishName.Contains(trimName, StringComparison.OrdinalIgnoreCase));
    }

    public Perk? Get(string name)
    {
        var trimName = name.Trim();
        return _list.FirstOrDefault(x => x.EnglishName.Contains(trimName, StringComparison.OrdinalIgnoreCase));
    }
}