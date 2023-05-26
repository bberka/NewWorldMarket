﻿using NewWorld.BiSMarket.Core.Models;
using NewWorld.BiSMarket.Core.Properties;

namespace NewWorld.BiSMarket.Core;

public class PerkMgr
{
    private PerkMgr()
    {
        var perkList = Resource.PerkListV2.Split(Environment.NewLine);
        _list = new List<Perk>();
        for (int i = 0; i < perkList.Length; i++)
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
			Instance ??= new();
			return Instance;
		}
	}
	private static PerkMgr? Instance;
	private List<Perk> _list;
    public IReadOnlyCollection<Perk> Perks => _list;
    public Perk? GetPerk(int id)
    {
        return _list.FirstOrDefault(x => x.Id == id);
    }

    public bool IsValidPerk(string name)
    {
        var nameTrim = name.Trim();
        return _list.Any(x => x.EnglishName.Equals(nameTrim, StringComparison.OrdinalIgnoreCase));
    }

    public Perk? GetPerk(string name)
    {
        var nameTrim = name.Trim();
        return _list.FirstOrDefault(x => x.EnglishName.Equals(nameTrim,StringComparison.OrdinalIgnoreCase));
    }
}