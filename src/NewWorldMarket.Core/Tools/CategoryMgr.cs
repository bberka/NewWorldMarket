using NewWorldMarket.Core.Constants;
using NewWorldMarket.Core.Models;

namespace NewWorldMarket.Core.Tools;

public class CategoryMgr
{
    private static CategoryMgr? Instance;

    public IReadOnlyCollection<Category> Categories;

    private CategoryMgr()
    {
        Categories = new List<Category>
        {
            //One Handed
            new(MainCategoryType.One__Handed_Weapons, ItemType.Hatchet),
            new(MainCategoryType.One__Handed_Weapons, ItemType.Sword),
            new(MainCategoryType.One__Handed_Weapons, ItemType.Rapier),

            //2 Handed
            new(MainCategoryType.Two__Handed_Weapons, ItemType.Spear),
            new(MainCategoryType.Two__Handed_Weapons, ItemType.Great_Axe),
            new(MainCategoryType.Two__Handed_Weapons, ItemType.Greatsword),
            new(MainCategoryType.Two__Handed_Weapons, ItemType.War_Hammer),

            //Ranged
            new(MainCategoryType.Ranged_Weapons, ItemType.Musket),
            new(MainCategoryType.Ranged_Weapons, ItemType.Bow),
            new(MainCategoryType.Ranged_Weapons, ItemType.Blunderbuss),

            //magic
            new(MainCategoryType.Magic_Weapons, ItemType.Fire_Staff),
            new(MainCategoryType.Magic_Weapons, ItemType.Life_Staff),
            new(MainCategoryType.Magic_Weapons, ItemType.Ice_Gauntlet),
            new(MainCategoryType.Magic_Weapons, ItemType.Void_Gauntlet),

            //Armors
            new(MainCategoryType.Armors, ItemType.Heavy_Chestwear),
            new(MainCategoryType.Armors, ItemType.Heavy_Footwear),
            new(MainCategoryType.Armors, ItemType.Heavy_Glove),
            new(MainCategoryType.Armors, ItemType.Heavy_Headwear),
            new(MainCategoryType.Armors, ItemType.Heavy_Legwear),
            new(MainCategoryType.Armors, ItemType.Medium_Chestwear),
            new(MainCategoryType.Armors, ItemType.Medium_Footwear),
            new(MainCategoryType.Armors, ItemType.Medium_Glove),
            new(MainCategoryType.Armors, ItemType.Medium_Headwear),
            new(MainCategoryType.Armors, ItemType.Medium_Legwear),
            new(MainCategoryType.Armors, ItemType.Light_Chestwear),
            new(MainCategoryType.Armors, ItemType.Light_Footwear),
            new(MainCategoryType.Armors, ItemType.Light_Glove),
            new(MainCategoryType.Armors, ItemType.Light_Headwear),
            new(MainCategoryType.Armors, ItemType.Light_Legwear),

            //Jewelry

            new(MainCategoryType.Jewelry, ItemType.Amulet),
            new(MainCategoryType.Jewelry, ItemType.Ring),
            new(MainCategoryType.Jewelry, ItemType.Earring),

            //Sheilds
            new(MainCategoryType.Shields, ItemType.Kite_Shield),
            new(MainCategoryType.Shields, ItemType.Round_Shield),
            new(MainCategoryType.Shields, ItemType.Tower_Shield)
        };
    }

    public static CategoryMgr This
    {
        get
        {
            Instance ??= new CategoryMgr();
            return Instance;
        }
    }

    public bool IsValidCategory(int mainCategory, int subCategory)
    {
        try
        {
            return Categories.Any(x =>
                x.CategoryType == (MainCategoryType)mainCategory && x.ItemType == (ItemType)subCategory);
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