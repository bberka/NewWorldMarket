using Microsoft.AspNetCore.Mvc;
using NewWorld.BiSMarket.Core.Abstract;

namespace NewWorld.BiSMarket.Web.Controllers.ApiControllers;

public class AccountController : BaseApiController
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult RemoveCharacter(Guid guid)
    {
        var user = SessionLib.This.GetUser();
        var registerResult = _userService.RemoveCharacter(user.Guid, guid);
        return Ok(registerResult);
    }
}