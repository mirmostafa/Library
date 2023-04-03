using System.Net;

namespace Library.Web.Helpers;

public static class HttpStatusCodeHelper
{
    public static HttpStatusCodeKind GetKind(this HttpStatusCode code)
        => code.Cast().ToInt() switch
        {
            <= 199 => HttpStatusCodeKind.Informational,
            <= 299 => HttpStatusCodeKind.Success,
            <= 399 => HttpStatusCodeKind.Redirection,
            <= 499 => HttpStatusCodeKind.ClientError,
            <= 599 => HttpStatusCodeKind.ServerError,
            _ => throw new NotImplementedException(),
        };

    public static bool IsSucceed(this HttpStatusCode code)
        => code.GetKind().IsIn(HttpStatusCodeKind.Success, HttpStatusCodeKind.Informational);

    public static HttpStatusCode ToHttpStatusCode(int code)
        => (HttpStatusCode)code;

    public static HttpStatusCode? ToHttpStatusCode(int? code)
        => code is null ? null : (HttpStatusCode)code;
}

public enum HttpStatusCodeKind
{
    Informational = 1,
    Success = 2,
    Redirection = 3,
    ClientError = 4,
    ServerError = 5,
}