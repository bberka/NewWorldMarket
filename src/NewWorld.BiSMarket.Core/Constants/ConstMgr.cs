using System.Diagnostics;
using System.Reflection;
using IronOcr;
using NewWorld.BiSMarket.Core.Models;

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
    public const string OcrIgnoredCharacters = "[]{}()@\\";
    public static OcrLanguage DefaultOcrLanguage = OcrLanguage.EnglishBest;
    public const int PageSize = 20;
    public const int MaxImageSize = 1048576;
    public const int DefaultExpirationTimeInHours = 24 * 14; //14 days
    public const int PremiumExpirationTimeInHours = 24 * 30; //30 days

    public const int DefaultOrderCountLimit = 5;
    public const int VerifiedOrderCountLimit = 10;
    public const int PremiumOrderCountLimit = 25;

    public const int DefaultOrderRequestCountLimit = 1;
    public const int VerifiedOrderRequestCountLimit = 3;
    public const int PremiumOrderRequestCountLimit = 5;

    public const int DefaultCharacterLimit = 2;
    public const int PremiumCharacterLimit = 5;




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