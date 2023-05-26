using EasMe;
using EasMe.Extensions;
using EasMe.Result;
using NewWorld.BiSMarket.Core.Abstract;
using NewWorld.BiSMarket.Core.Entity;
using NewWorld.BiSMarket.Core.Models;
using System.Drawing;
using NewWorld.BiSMarket.Core;
using NewWorld.BiSMarket.Core.Constants;

namespace NewWorld.BiSMarket.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public ResultData<Guid> Register(RegisterRequest request)
    {
        var emailExists = _unitOfWork.UserRepository.Any(x => x.Email == request.Email);
        if (emailExists)
        {
            return Result.Warn("Email already used by another account");
        }
        if (!request.Password.Equals(request.PasswordConfirm, StringComparison.Ordinal))
        {
            return Result.Warn("Passwords do not match");
        }
        var usernameExists = _unitOfWork.UserRepository.Any(x => x.Username == request.Username);
        if (usernameExists)
        {
            return Result.Warn("Username already used by another account");
        }
        //var isValidRegion = ServerMgr.This.IsValidServer(request.Region, request.Server);
        //if (!isValidRegion)
        //{
        //    return Result.Warn("Invalid region or server");
        //}

        var userEntity = new User();
        userEntity.Username = request.Username;
        userEntity.Email = request.Email;
        userEntity.PasswordHash = request.Password.MD5Hash().ToBase64String();
        userEntity.RegisterDate = DateTime.Now;
        _unitOfWork.UserRepository.Insert(userEntity);
        var result = _unitOfWork.Save();
        if (result.IsFailure)
        {
            return Result.Error(ErrCode.InternalDbError.ToMessage());
        }
        return userEntity.Guid;
    }

    public ResultData<Guid> AddCharacter(AddCharacter request)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == request.UserGuid);
        if (user == null)
        {
            return Result.Warn("Invalid user");
        }
        var sameCharExist = _unitOfWork.CharacterRepository
            .Any(x => x.Name == request.Name && x.Server == request.Server && x.Region == request.Region);
        if (sameCharExist)
        {
            return Result.Warn("Same character already exists in our data, if you are the owner of this character contact us to verify your account.");
        }
        var character = new Character
        {
            Name = request.Name,
            Server = request.Server,
            Region = request.Region,
            UserGuid = request.UserGuid,
            RegisterDate = DateTime.Now,
        };
        _unitOfWork.CharacterRepository.Insert(character);
        var result = _unitOfWork.Save();
        if (result.IsFailure)
        {
            return Result.Error(ErrCode.InternalDbError.ToMessage());
        }
        return character.Guid;
    }

    public Result RemoveCharacter(RemoveCharacter request)
    {
        var character = _unitOfWork.CharacterRepository.GetFirstOrDefault(x => x.Guid == request.CharacterGuid);
        if (character == null)
        {
            return Result.Warn("Invalid character");
        }
        var isOwner = character.UserGuid == request.UserGuid;
        if (!isOwner)
        {
            return Result.Warn("You are not the owner of this character");
        }
        _unitOfWork.CharacterRepository.Delete(character);
        var result = _unitOfWork.Save();
        return result;
    }

    public ResultData<User> Login(string username, string password)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Username == username);
        if (user == null)
        {
            return Result.Warn("Invalid username or password");
        }
        var passwordHash = password.MD5Hash().ToBase64String();
        if (user.PasswordHash != passwordHash)
        {
            return Result.Warn("Invalid username or password");
        }
        user.LastLoginDate = DateTime.Now;
        _unitOfWork.UserRepository.Update(user);
        var result = _unitOfWork.Save();
        if (result.IsFailure)
        {
            return Result.Error(ErrCode.InternalDbError.ToMessage());
        }
        return user;
    }

    public ResultData<User> GetUserByGuid(Guid guid)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == guid);
        if (user == null)
        {
            return Result.Warn("User not found");
        }
        return user;

    }

    public ResultData<User> GetUserByUsername(string username)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Username == username);
        if (user == null)
        {
            return Result.Warn("User not found");
        }
        return user;
    }

    public ResultData<User> GetUserByEmail(string email)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Email == email);
        if (user == null)
        {
            return Result.Warn("User not found");
        }
        return user;
    }

    public ResultData<User> GetUserByDiscordId(string discordId)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.DiscordId == discordId);
        if (user == null)
        {
            return Result.Warn("User not found");
        }
        return user;
    }

    public ResultData<User> GetUserBySteamId(string steamId)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.SteamId == steamId);
        if (user == null)
        {
            return Result.Warn("User not found");
        }
        return user;
    }



    public Result ChangePassword(Guid userGuid, ChangePassword request)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == userGuid);
        if (user == null)
        {
            return Result.Warn("User not found");
        }
        var passwordHash = request.CurrentPassword.MD5Hash().ToBase64String();
        if (user.PasswordHash != passwordHash)
        {
            return Result.Warn("Current password is not correct");
        }
        if (!request.NewPassword.Equals(request.NewPasswordConfirm, StringComparison.Ordinal))
        {
            return Result.Warn("Passwords do not match");
        }
        user.PasswordHash = request.NewPassword.MD5Hash().ToBase64String();
        _unitOfWork.UserRepository.Update(user);
        var result = _unitOfWork.Save();
        if (result.IsFailure)
        {
            return Result.Error(ErrCode.InternalDbError.ToMessage());
        }
        return Result.Success();
    }

    public Result ChangeEmail(Guid userGuid, string email)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == userGuid);
        if (user == null)
        {
            return Result.Warn("User not found");
        }
        var emailExists = _unitOfWork.UserRepository.Any(x => x.Email == email);
        if (emailExists)
        {
            return Result.Warn("Email already used by another account");
        }
        user.Email = email;
        _unitOfWork.UserRepository.Update(user);
        var result = _unitOfWork.Save();
        if (result.IsFailure)
        {
            return Result.Error(ErrCode.InternalDbError.ToMessage());
        }
        return Result.Success();
    }

    public Result ChangeDiscordId(Guid userGuid, string discordId)
    {

        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == userGuid);
        if (user == null)
        {
            return Result.Warn("User not found");
        }
        var discordIdExists = _unitOfWork.UserRepository.Any(x => x.DiscordId == discordId);
        if (discordIdExists)
        {
            return Result.Warn("Discord ID already used by another account");
        }
        user.DiscordId = discordId;
        _unitOfWork.UserRepository.Update(user);
        var result = _unitOfWork.Save();
        if (result.IsFailure)
        {
            return Result.Error(ErrCode.InternalDbError.ToMessage());
        }
        return Result.Success();
    }

    public Result VerifyEmail(string token)
    {
        throw new NotImplementedException();
    }

    public Result SendVerificationEmail(Guid userGuid)
    {
        throw new NotImplementedException();

    }
}