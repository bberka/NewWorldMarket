using System.Text.RegularExpressions;
using EasMe.Extensions;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NewWorld.BiSMarket.Web.Filters
{
    internal class ExceptionHandleFilter : IExceptionFilter
    {
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public void OnException(ExceptionContext context)
        {
            var query = context.HttpContext.Request.QueryString;
            logger.Exception(context.Exception, $"Query({query})");
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 500;
        }
    }
}
