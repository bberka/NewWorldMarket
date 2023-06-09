using EasMe.Result;

namespace NewWorldMarket.Core.Abstract;

public interface IMarketEmailClient
{
    Result SendEmailVerification(string email, string username, string token);
    Result SendPasswordReset(string email, string username, string token);

    Result SendOrderCancelledByAdmin(string email, string username, string orderId, string reason,
        string itemDataString);

    Result SendOrderExpired(string email, string username, string orderId, string itemDataString);
    Result SendUnknownIpAddressLogin(string email, string username, string ipAddress);
    Result SendDiscordLinked(string email, string username, string discordId, string discordUsername);
    Result SendSteamLinked(string email, string username, string steamId, string steamUsername);
    Result SendDiscordUnlinked(string email, string username, string discordId, string discordUsername);
    Result SendSteamUnlinked(string email, string username, string steamId, string steamUsername);
    Result SendPasswordChanged(string email, string username, string ipAddress);
}