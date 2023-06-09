using System.Security.Claims;
using EasMe;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NewWorldMarket.Entities;

namespace NewWorldMarket.Core.Tools;

public class SessionLib
{
    private static SessionLib? Instance;
    private static EasJWT _jwt;

    private SessionLib()
    {
        var jwtSecret = EasConfig.GetString("JwtToken");
        _jwt = new EasJWT(jwtSecret);
    }

    public static SessionLib This
    {
        get
        {
            Instance ??= new SessionLib();
            return Instance;
        }
    }

    public User? GetUser()
    {
        var context = HttpContextHelper.Current;
        if (context?.User.Identity is not ClaimsIdentity identity) return null;
        var json = identity.FindFirst("UserJson")?.Value;
        if (json is null) return null;
        var user = JsonConvert.DeserializeObject<User>(json);
        return user;
    }

    public void SetUser(User user)
    {
        var context = HttpContextHelper.Current;

        var json = JsonConvert.SerializeObject(user, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        var dic = new Dictionary<string, object?>
        {
            { "UserJson", json },
            { ClaimTypes.Name, user.Username }
        };
        var token = _jwt.GenerateJwtToken(dic, 1440);
        context?.Session.SetString("token", token);
    }

    public void ClearSession()
    {
        var context = HttpContextHelper.Current;

        context?.Session.Remove("token");
    }

    public bool IsAuthenticated()
    {
        var context = HttpContextHelper.Current;
        var isAuthenticated = context?.User?.Identity?.IsAuthenticated;
        if (isAuthenticated is null) return false;
        return true;
    }
}