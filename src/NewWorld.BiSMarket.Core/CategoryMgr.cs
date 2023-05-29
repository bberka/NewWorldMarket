using NewWorld.BiSMarket.Core.Constants;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Core;

public class CategoryMgr
{

    private CategoryMgr()
    {
        Categories = new List<Category>()
        {
			//One Handed
			new Category(MainCategoryType.One__Handed_Weapons, ItemType.Hatchet),
			new Category(MainCategoryType.One__Handed_Weapons, ItemType.Sword),
			new Category(MainCategoryType.One__Handed_Weapons, ItemType.Rapier),

			//2 Handed
			new Category(MainCategoryType.Two__Handed_Weapons, ItemType.Spear),
			new Category(MainCategoryType.Two__Handed_Weapons, ItemType.Great_Axe),
			new Category(MainCategoryType.Two__Handed_Weapons, ItemType.Greatsword),
			new Category(MainCategoryType.Two__Handed_Weapons, ItemType.War_Hammer),

			//Ranged
			new Category(MainCategoryType.Ranged_Weapons, ItemType.Musket),
			new Category(MainCategoryType.Ranged_Weapons, ItemType.Bow),
			new Category(MainCategoryType.Ranged_Weapons, ItemType.Blunderbuss),

			//magic
			new Category(MainCategoryType.Magic_Weapons, ItemType.Fire_Staff),
			new Category(MainCategoryType.Magic_Weapons, ItemType.Life_Staff),
			new Category(MainCategoryType.Magic_Weapons, ItemType.Ice_Gauntlet),
			new Category(MainCategoryType.Magic_Weapons, ItemType.Void_Gauntlet),

			//Armors
            new Category(MainCategoryType.Armors, ItemType.Heavy_Chestwear),
            new Category(MainCategoryType.Armors, ItemType.Heavy_Footwear),
            new Category(MainCategoryType.Armors, ItemType.Heavy_Glove),
            new Category(MainCategoryType.Armors, ItemType.Heavy_Headwear),
            new Category(MainCategoryType.Armors, ItemType.Heavy_Legwear),
            new Category(MainCategoryType.Armors, ItemType.Medium_Chestwear),
            new Category(MainCategoryType.Armors, ItemType.Medium_Footwear),
            new Category(MainCategoryType.Armors, ItemType.Medium_Glove),
            new Category(MainCategoryType.Armors, ItemType.Medium_Headwear),
            new Category(MainCategoryType.Armors, ItemType.Medium_Legwear),
            new Category(MainCategoryType.Armors, ItemType.Light_Chestwear),
            new Category(MainCategoryType.Armors, ItemType.Light_Footwear),
            new Category(MainCategoryType.Armors, ItemType.Light_Glove),
            new Category(MainCategoryType.Armors, ItemType.Light_Headwear),
            new Category(MainCategoryType.Armors, ItemType.Light_Legwear),

			//Jewelry

            new Category(MainCategoryType.Jewelry, ItemType.Amulet),
            new Category(MainCategoryType.Jewelry, ItemType.Ring),
            new Category(MainCategoryType.Jewelry, ItemType.Earring),

			//Sheilds
            new Category(MainCategoryType.Shields, ItemType.Kite_Shield),
            new Category(MainCategoryType.Shields, ItemType.Round_Shield),
            new Category(MainCategoryType.Shields, ItemType.Tower_Shield),

        };

    }
	public static CategoryMgr This
	{
		get
		{
			Instance ??= new();
			return Instance;
		}
	}
	private static CategoryMgr? Instance;

    public IReadOnlyCollection<Category> Categories;

    public bool IsValidCategory(int mainCategory, int subCategory)
    {
        try
        {
            return Categories.Any(x => x.CategoryType == (MainCategoryType)mainCategory && x.ItemType == (ItemType)subCategory);

        }
        catch
        {
            return false;
        }
    }

    public List<Category> GetSubCategories(int mainCategory)
    {
        try
        {
            return Categories.Where(x => x.CategoryType == (MainCategoryType)mainCategory).ToList();

        }
        catch 
        {
            return new List<Category>();
        }
    }

    public List<Category> GetSubCategories(MainCategoryType type)
    {
        return Categories.Where(x => x.CategoryType == type).ToList();
    }

    public IReadOnlyDictionary<int, string> GetMainCategoriesAsDictionary()
    {
        var enumTypeAsDictionary = Enum.GetValues(typeof(MainCategoryType))
            .Cast<MainCategoryType>()
            .ToDictionary(t => (int)t, t => t.ToString());
        return enumTypeAsDictionary;
    }


}