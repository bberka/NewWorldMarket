using Microsoft.AspNetCore.Http;
using NewWorldMarket.Core.Models;

namespace NewWorldMarket.Core.Tools;

public static class ContextMgr
{
    public static RequestInformation GetInfo(this HttpContext ctx)
    {
        var info = new RequestInformation();
        try
        {
            info.RemoteIpAddress = ctx.Connection.RemoteIpAddress?.ToString();
            info.UserAgent = ctx.Request.Headers["User-Agent"].ToString();
            info.XRealIpAddress = ctx.Request.Headers["X-Real-IP"].ToString();
            info.XForwardedForIpAddress = ctx.Request.Headers["X-Forwarded-For"].ToString();
            info.CfConnectingIpAddress = ctx.Request.Headers["CF-Connecting-IP"].ToString();
        }
        catch (Exception ex)
        {
            //TODO: logging
        }
        return info;
    }
}