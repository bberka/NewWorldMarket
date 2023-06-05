

namespace NewWorld.BiSMarket.Business.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ResultData<User> Register(Register request)
    {
        if (!request.Password.Equals(request.PasswordConfirm, StringComparison.Ordinal))
            return Result.Error("Passwords do not match");
        if (request.Email.Contains(" ")) return Result.Error("Email cannot contain spaces");
        if (request.Username.Contains(" ")) return Result.Error("Username cannot contain spaces");
        var isAllLetterAndNumber = request.Username.All(x => char.IsLetter(x) || char.IsNumber(x));
        if (!isAllLetterAndNumber) return Result.Error("Username can only contain letters and numbers");


        var emailExists = _unitOfWork.UserRepository.Any(x => x.Email == request.Email);
        if (emailExists) return Result.Error("Email already used by another account");
        var usernameExists = _unitOfWork.UserRepository.Any(x => x.Username == request.Username);
        if (usernameExists) return Result.Error("Username already used by another account");
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
        if (result.IsFailure) return Result.Error(ErrCode.InternalDbError.ToMessage());
        return userEntity;
    }

    public ResultData<Guid> AddCharacter(AddCharacter request)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == request.UserGuid);
        if (user == null) return Result.Warn("Invalid user");
        var isValidServer = ServerMgr.This.IsValidServer(request.Region, request.Server);
        if (!isValidServer) return Result.Warn("Invalid region or server");
        var sameCharExist = _unitOfWork.CharacterRepository
            .Any(x => x.Name == request.Name && x.Server == request.Server && x.Region == request.Region);
        if (sameCharExist)
            return Result.Warn(
                "Same character already exists in our data, if you are the owner of this character contact us.");
        var characterCount =
            _unitOfWork.CharacterRepository.Count(x => x.UserGuid == request.UserGuid && !x.DeletedDate.HasValue);
        if (characterCount >= ConstMgr.DefaultCharacterLimit)
            return Result.Warn($"You can only add {ConstMgr.DefaultCharacterLimit} characters");
        var character = new Character
        {
            Name = request.Name,
            Server = request.Server,
            Region = request.Region,
            UserGuid = request.UserGuid,
            RegisterDate = DateTime.Now
        };
        _unitOfWork.CharacterRepository.Insert(character);
        var result = _unitOfWork.Save();
        if (result.IsFailure) return Result.Error(ErrCode.InternalDbError.ToMessage());
        return character.Guid;
    }

    public Result RemoveCharacter(Guid userGuid, Guid characterGuid)
    {
        var character = _unitOfWork.CharacterRepository.GetFirstOrDefault(x => x.Guid == characterGuid);
        if (character == null) return Result.Warn("Invalid character");
        var isOwner = character.UserGuid == userGuid;
        if (!isOwner)
            return DomainResult.Unauthorized;
        var orderList = _unitOfWork.OrderRepository.Get(x => x.CharacterGuid == characterGuid
                                                             && !x.CancelledDate.HasValue
                                                             && !x.CompletedDate.HasValue
                                                             && x.ExpirationDate > DateTime.Now).ToList();
        //invalidate orders
        if (orderList.Count > 0)
        {
            foreach (var order in orderList) order.CancelledDate = DateTime.Now;
            _unitOfWork.OrderRepository.UpdateRange(orderList);
        }

        character.DeletedDate = DateTime.Now;
        _unitOfWork.CharacterRepository.Update(character);
        var result = _unitOfWork.Save();
        return result;
    }

    public ResultData<User> Login(string username, string password)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Username == username);
        if (user == null) return Result.Warn("Invalid username or password");
        var passwordHash = password.MD5Hash().ToBase64String();
        if (user.PasswordHash != passwordHash) return Result.Warn("Invalid username or password");
        user.LastLoginDate = DateTime.Now;
        _unitOfWork.UserRepository.Update(user);
        var result = _unitOfWork.Save();
        if (result.IsFailure) return Result.Error(ErrCode.InternalDbError.ToMessage());
        return user;
    }

    public ResultData<User> GetUserByGuid(Guid guid)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == guid);
        if (user == null) return Result.Warn("User not found");
        return user;
    }

    public ResultData<List<Character>> GetCharacters(Guid userGuid)
    {
        var characters = _unitOfWork.CharacterRepository.Get(x => x.UserGuid == userGuid && !x.DeletedDate.HasValue)
            .ToList();
        return characters;
    }

    public ResultData<User> GetUserByUsername(string username)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Username == username);
        if (user == null) return Result.Warn("User not found");
        return user;
    }

    public ResultData<User> GetUserByEmail(string email)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Email == email);
        if (user == null) return Result.Warn("User not found");
        return user;
    }

    public ResultData<User> GetUserByDiscordId(string discordId)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.DiscordId == discordId);
        if (user == null) return Result.Warn("User not found");
        return user;
    }

    public ResultData<User> GetUserBySteamId(string steamId)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.SteamId == steamId);
        if (user == null) return Result.Warn("User not found");
        return user;
    }


    public Result ChangePassword(Guid userGuid, ChangePassword request)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == userGuid);
        if (user == null) return Result.Warn("User not found");
        var passwordHash = request.CurrentPassword.MD5Hash().ToBase64String();
        if (user.PasswordHash != passwordHash) return Result.Warn("Current password is not correct");
        if (!request.NewPassword.Equals(request.NewPasswordConfirm, StringComparison.Ordinal))
            return Result.Warn("Passwords do not match");
        user.PasswordHash = request.NewPassword.MD5Hash().ToBase64String();
        _unitOfWork.UserRepository.Update(user);
        var result = _unitOfWork.Save();
        if (result.IsFailure) return Result.Error(ErrCode.InternalDbError.ToMessage());
        return Result.Success();
    }

    public Result ChangeEmail(Guid userGuid, string email)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == userGuid);
        if (user == null) return Result.Warn("User not found");
        var emailExists = _unitOfWork.UserRepository.Any(x => x.Email == email);
        if (emailExists) return Result.Warn("Email already used by another account");
        user.Email = email;
        _unitOfWork.UserRepository.Update(user);
        var result = _unitOfWork.Save();
        if (result.IsFailure) return Result.Error(ErrCode.InternalDbError.ToMessage());
        return Result.Success();
    }

    public Result ChangeDiscordId(Guid userGuid, string discordId)
    {
        var user = _unitOfWork.UserRepository.GetFirstOrDefault(x => x.Guid == userGuid);
        if (user == null) return Result.Warn("User not found");
        var discordIdExists = _unitOfWork.UserRepository.Any(x => x.DiscordId == discordId);
        if (discordIdExists) return Result.Warn("Discord ID already used by another account");
        user.DiscordId = discordId;
        _unitOfWork.UserRepository.Update(user);
        var result = _unitOfWork.Save();
        if (result.IsFailure) return Result.Error(ErrCode.InternalDbError.ToMessage());
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