using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewWorldMarket.Core.Constants;

namespace NewWorldMarket.Web.Controllers.ApiControllers;

[Authorize]
public class AccountController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly IFileLogger _fileLogger;
    private readonly ILogService _logService;

    public AccountController(
        IUserService userService,
        IFileLogger fileLogger,
        ILogService logService)
    {
        _userService = userService;
        _fileLogger = fileLogger;
        _logService = logService;
    }

    [HttpGet]
    public IActionResult RemoveCharacter(Guid guid)
    {
        var user = SessionLib.This.GetUser();
        var result = _userService.RemoveCharacter(user.Guid, guid);
        _fileLogger.Log(ActionType.AccountRemoveCharacter, result.Severity, result.ErrorCode);
        _logService.Log(ActionType.AccountRemoveCharacter, result.Severity, result.ErrorCode);
        return Ok(result);
    }
}