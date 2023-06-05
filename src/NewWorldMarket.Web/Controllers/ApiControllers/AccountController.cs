using Microsoft.AspNetCore.Mvc;

namespace NewWorldMarket.Web.Controllers.ApiControllers;

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