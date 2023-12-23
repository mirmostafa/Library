using System.Diagnostics.Contracts;

using Library.Results;
using Library.Validations;

using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

namespace Library.Web.Security.Identity.Model.Tokens;

public sealed class Jwt
{
    [Pure]
    [return: NotNull]
    public static Result<IEnumerable<(string Key, string? Value)>> DecodeJwt([DisallowNull] string jwtToken)
    {
        var vr = Check.IfArgumentIsNull(jwtToken);
        if (vr.IsFailure)
        {
            return vr.WithValue(Enumerable.Empty<(string, string?)>());
        }
        var parts = jwtToken.Split('.');
        if (parts.Length != 3)
        {
            return Result<IEnumerable<(string, string?)>>.CreateFailure(new Library.Exceptions.Validations.ValidationException("Invalid format"), default!);
        }
        var headerJson = Base64UrlEncoder.Decode(parts[0]);
        var payloadJson = Base64UrlEncoder.Decode(parts[1]);
        try
        {
            var headerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(headerJson);
            var payloadData = JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadJson);

            return new(parse(headerData, payloadData));
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<(string, string?)>>.CreateFailure(ex, default!);
        }

        static IEnumerable<(string Key, string? Value)> parse(Dictionary<string, object> headerData, Dictionary<string, object> payloadData)
        {
            foreach (var item in headerData)
            {
                yield return (item.Key, item.Value?.ToString());
            }

            foreach (var item in payloadData)
            {
                yield return (item.Key, item.Value?.ToString());
            }
        }
    }
}