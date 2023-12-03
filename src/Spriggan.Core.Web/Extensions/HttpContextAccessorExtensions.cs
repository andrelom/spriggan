using System.Net;
using Microsoft.AspNetCore.Http;

namespace Spriggan.Core.Web.Extensions;

public static class HttpContextAccessorExtensions
{
    public static IPAddress? GetIpAddress(this HttpContext context)
    {
        return GetIpAddressByHeader("X-FORWARDED-FOR", context)
               ?? GetIpAddressByHeader("REMOTE_ADDR", context)
               ?? context.Connection.RemoteIpAddress;
    }

    #region Private Methods

    private static IPAddress? GetIpAddressByHeader(string name, HttpContext context)
    {
        if (context.Request.Headers[name] is { } values
            && values.Select(value => value?.Trim()).FirstOrDefault() is { } data
            && IPAddress.TryParse(data, out var ip))
        {
            return ip;
        }

        return null;
    }

    #endregion
}
