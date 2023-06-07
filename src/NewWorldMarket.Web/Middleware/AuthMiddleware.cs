using System.Net;
using EasMe.Extensions;
using EasMe.Logging;
using EasMe.Result;

namespace NewWorldMarket.Web.Middleware;

public class AuthMiddleware
{
    private static readonly IEasLog logger = EasLogFactory.CreateLogger();

    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var uri = context.Request.GetRequestQuery();
        var accessToken = context.Session.GetString("token");
        if (accessToken is not null && accessToken != string.Empty)
            context.Request.Headers.Add("Authorization", "Bearer " + accessToken);
        if (uri.Contains("Embeds") && context.Session.GetString("token") is null)
            context.Response.StatusCode = 401;
        else
            await _next(context);

        var status = context.Response.StatusCode;
        if (status == (int)HttpStatusCode.Unauthorized || status == (int)HttpStatusCode.Forbidden)
            context.Response.Redirect("/");
#if !DEBUG
            else if (status > 403)
			{
				context.Response.Redirect("/"); // + context.Response.StatusCode
        }
#endif
        var isApiRequest = uri.Contains("/api/");
        if (status >= 400 && isApiRequest)
        {
            //context.Response.StatusCode = 200;
            //context.Response.ContentType = "application/json";
            //await context.Response.WriteAsync(Result.Error("Status Code Error: " + status).ToJsonString());

            //logger.Error($"[{status}] {uri}");
        }
    }
}