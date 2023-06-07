using EasMe;

namespace NewWorldMarket.Core.Constants;

public static class LinkMgr
{
    public static string GithubUrl => EasConfig.GetString("GithubUrl") ?? "#";
    
    public static string DiscordUrl =>  EasConfig.GetString("DiscordUrl") ?? "#";
    
    public static string PatreonUrl => EasConfig.GetString("PatreonUrl") ?? "#";
    public static string BuyMeCoffeeUrlUrl => EasConfig.GetString("BuyMeCoffeeUrl") ?? "#";
   
}