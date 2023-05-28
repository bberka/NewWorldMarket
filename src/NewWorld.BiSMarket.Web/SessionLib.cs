using EasMe;
using EasMe.Extensions;
using Newtonsoft.Json;
using NewWorld.BiSMarket.Core.Entity;
using System.Security.Claims;

namespace NewWorld.BiSMarket.Web;

public class SessionLib
{

    private SessionLib()
    {
        var jwtSecret = EasConfig.GetString("JwtToken");
        _jwt = new(jwtSecret);
    }
    public static SessionLib This
    {
        get
        {
            Instance ??= new();
            return Instance;
        }
    }
    private static SessionLib? Instance;
    private static EasJWT _jwt;
    public User? GetUser()
    {
        var context = new HttpContextAccessor();
        if (context.HttpContext?.User.Identity is not ClaimsIdentity identity) return null;
        var json = identity.FindFirst("UserJson")?.Value;
        if (json is null) return null;
        var user = JsonConvert.DeserializeObject<User>(json);
        return user;
    }

    public void SetUser(User user)
    {
        var context = new HttpContextAccessor();
        var json = JsonConvert.SerializeObject(user, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        var dic = new Dictionary<string, object?>()
        {
            { "UserJson", json },
            { ClaimTypes.Name, user.Username},
        };
        var token = _jwt.GenerateJwtToken(dic, 1440);
        context.HttpContext?.Session.SetString("token", token);
    }

    public void ClearSession()
    {
        var context = new HttpContextAccessor();
        context.HttpContext?.Session.Remove("token");

    }
    public bool IsAuthenticated()
    {
        var context = new HttpContextAccessor();
        var isAuthenticated = context.HttpContext?.User?.Identity?.IsAuthenticated;
        if (isAuthenticated is null)
        {
            return false;
        }
        return true;
    }
}