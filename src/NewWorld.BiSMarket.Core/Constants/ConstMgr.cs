using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using IronOcr;
using NewWorld.BiSMarket.Core.Models;
using NewWorld.BiSMarket.Core.Properties;
using Color = IronSoftware.Drawing.Color;
using Region = NewWorld.BiSMarket.Core.Models.Region;

namespace NewWorld.BiSMarket.Core.Constants;

public static class ConstMgr
{
    public static bool IsDevelopment
    {
        get
        {
#if DEBUG || DEVELOPMENT
            return true;
#else
            return false;
#endif
        }
    }

    public static string? Version
    {
        get
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetName().Version?.ToString();
        }
    }

    internal const string ApiKey = "K82989277388957";
    public const string OcrIgnoredCharacters = "[]{}()@\\-—_=";
    public static OcrLanguage DefaultOcrLanguage = OcrLanguage.EnglishBest;
    public const int PageSize = 20;
    public const int MaxImageSize = 1048576;
    public const int DefaultExpirationTimeInHours = 24 * 14; //14 days
    public const int PremiumExpirationTimeInHours = 24 * 30; //30 days

    public const int DefaultOrderCountLimit = 10;
    public const int VerifiedOrderCountLimit = 20;
    public const int PremiumOrderCountLimit = 50;

    public const int DefaultOrderRequestCountLimit = 2;
    public const int VerifiedOrderRequestCountLimit = 5;
    public const int PremiumOrderRequestCountLimit = 10;

    public const int DefaultCharacterLimit = 2;
    public const int PremiumCharacterLimit = 5;

    public const float MaxPriceLimit = 500_000.000F;

    public const int MaxDeliveryTime = 48;
    public static IReadOnlyCollection<IronSoftware.Drawing.Color> ItemTooltipTextColorList = new List<Color>()
    {
        new Color(14,127,201), //Blue - Perks and attribute color
        new Color(255,255,255), //White - Desc
        new Color(169,169,169), //Gray - Some of the texts in desc
        new Color(255,255,113), //Bright Yellow - Legendary text
        new Color(255,174,60), // Orange - Legendary item name text
        new Color(255,106,106), // Red-ish - When GearScore is lower than equipped item
        new Color(250,186,250), // Bright Pink - Epic text
        new Color(255,73,255), // Pink  - Epic item name text
        new Color(60,243,184), // Bright Green - When GearScore is higher than equipped item
    };



    public static IReadOnlyCollection<Region> Regions = new List<Region>()
    {
        new(1, "EU Central",
            new Server(1, "Nyx"),
            new Server(2, "Kronos"),
            new Server(3, "Nysa"),
            new Server(4, "Abaton"),
            new Server(5, "Barri"),
            new Server(6, "Asgard"),
            new Server(7, "Aaru")
        ),
        new(2,"US East",
            new Server(8,"Lilith"),
            new Server(9,"Maramma"),
            new Server(10,"Valhalla"),
            new Server(11,"Castle of Steel")
        ),
        new(3, "US West",
            new Server(12,"Isabella"),
            new Server(13,"El Dorado")
            ),
        new(4, "SA East",
            new Server(14,"Artorius"),
            new Server(15,"Devaloka")
            ),
        new(5, "AP Southeast",
            new Server(16,"Delos"),
            new Server(17,"Sutekh")
            ),
    };
}