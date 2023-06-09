using EasMe.Result;
using NewWorldMarket.Core.Abstract;

namespace NewWorldMarket.Core.Tools;

public class MarketEmailClient : IMarketEmailClient
{

	private MarketEmailClient() { }
	public static MarketEmailClient This
	{
		get
		{
			Instance ??= new();
			return Instance;
		}
	}
	private static MarketEmailClient? Instance;
    public Result SendEmailVerification(string email, string username, string token)
    {
        return DomainResult.NotImplemented;
    }

    public Result SendPasswordReset(string email, string username, string token)
    {
        return DomainResult.NotImplemented;
    }

    public Result SendOrderCancelledByAdmin(string email, string username, string orderId, string reason,
        string itemDataString)
    {
        return DomainResult.NotImplemented;
    }

    //public Result SendOrderListed(string email, string username, string orderId, string itemDataString)
    //{
    //    return DomainResult.NotImplemented;
    //}

    //public Result SendOrderSold(string email, string username, string orderId, string itemDataString)
    //{
    //    return DomainResult.NotImplemented;
    //}

    //public Result SendOrderPriceUpdated(string email, string username, string orderId, string itemDataString,
    //    float oldPrice, float newPrice)
    //{
    //    return DomainResult.NotImplemented;
    //}

    public Result SendOrderExpired(string email, string username, string orderId, string itemDataString)
    {
        return DomainResult.NotImplemented;
    }

    public Result SendUnknownIpAddressLogin(string email, string username, string ipAddress)
    {
        return DomainResult.NotImplemented;
    }

    public Result SendDiscordLinked(string email, string username, string discordId, string discordUsername)
    {
        return DomainResult.NotImplemented;

    }
    public Result SendSteamLinked(string email, string username, string steamId, string steamUsername)
    {
        return DomainResult.NotImplemented;


    }

    public Result SendDiscordUnlinked(string email, string username, string discordId, string discordUsername)
    {
        return DomainResult.NotImplemented;

    }

    public Result SendSteamUnlinked(string email, string username, string steamId, string steamUsername)
    {
        return DomainResult.NotImplemented;

    }

    public Result SendPasswordChanged(string email, string username,string ipAddress)
    {
        return DomainResult.NotImplemented;
    }
  
}