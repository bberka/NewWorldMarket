using EasMe.Result;
using Microsoft.AspNetCore.Http;
using NewWorld.BiSMarket.Core.Entity;
using NewWorld.BiSMarket.Core.Models;

namespace NewWorld.BiSMarket.Core.Abstract;

public interface IUserService
{
    ResultData<Guid> Register(Register request);
    ResultData<Guid> AddCharacter(AddCharacter request);
    Result RemoveCharacter(RemoveCharacter request);
    ResultData<User> Login(string username, string password);
    ResultData<User> GetUserByGuid(Guid guid);
    ResultData<User> GetUserByUsername(string username);
    ResultData<User> GetUserByEmail(string email);
    ResultData<User> GetUserByDiscordId(string discordId);
    ResultData<User> GetUserBySteamId(string steamId);
    Result ChangePassword(Guid userGuid, ChangePassword request);
    Result ChangeEmail(Guid userGuid, string email);
    Result ChangeDiscordId(Guid userGuid, string discordId);
    Result VerifyEmail(string token);
    Result SendVerificationEmail(Guid userGuid);
}