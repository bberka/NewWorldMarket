using EasMe.Result;
using NewWorldMarket.Entities;
using NewWorldMarket.Core.Models;

namespace NewWorldMarket.Core.Abstract;

public interface IUserService
{
    ResultData<User> Register(Register request);
    ResultData<Guid> AddCharacter(AddCharacter request);
    Result RemoveCharacter(Guid userGuid, Guid characterGuid);
    ResultData<User> Login(string username, string password);
    ResultData<User> GetUserByGuid(Guid guid);
    ResultData<List<Character>> GetCharacters(Guid userGuid);
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