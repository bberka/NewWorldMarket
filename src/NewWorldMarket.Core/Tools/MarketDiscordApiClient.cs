using EasMe.Result;
using NewWorldMarket.Core.Models;

namespace NewWorldMarket.Core.Tools;

public class MarketDiscordApiClient
{

    private MarketDiscordApiClient()
    {

    }
    public static MarketDiscordApiClient This
    {
        get
        {
            Instance ??= new();
            return Instance;
        }
    }
    private static MarketDiscordApiClient? Instance;
    private string _apiSecret;
    
    private string _apiUrl;
    public Result SendAnnouncement(string author,string message)
    {
        throw new NotImplementedException();
    }

    public Result SendSellOrderListed(ItemV3 itemData, string seller, string region, string server, string iconUrl,
        string fullImageUrl)
    {
        throw new NotImplementedException();

    }
    public Result SendBuyOrderListed(ItemV3 itemData, string buyer, string region, string server)
    {
        throw new NotImplementedException();
    }
}