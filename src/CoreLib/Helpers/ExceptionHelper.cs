using Library.Validations;

namespace Library.Helpers;

public static class ExceptionHelper
{
    public static string? GetFullMessage(this Exception? exception, Func<Exception, string> format, string? separator)
    {
        if (exception == null)
        {
            return null;
        }

        var message = format(exception);
        var inner = GetFullMessage(exception.InnerException, format, separator);
        return inner == null ? message : $"{message}{separator}{inner}";
    }

    public static string GetFullMessage(this Exception? exception, bool inculdeStackTrace = false)
    {
        Action<Exception, StringBuilder> addStackTrace = inculdeStackTrace
            ? ((ex, sb) =>
            {
                _ = sb.AppendLine($"Source: {ex.GetBaseException().Source}")
                  .AppendLine("=============Stack Trace=============")
                  .AppendLine(ex.StackTrace);
            })
            : ((_, _) => { });
        return GetFullMessage(exception, ex =>
        {
            var result = new StringBuilder($"{ex.Message ?? "(NO EXCEPTION MESSAGE)"}");
            addStackTrace(ex, result);
            return result.ToString();
        });
    }

    public static string GetFullMessage(this Exception? exception, in Func<Exception, string> exceptionFormatter)
    {
        if (exception is null)
        {
            return string.Empty;
        }
        Check.MustBeArgumentNotNull(exceptionFormatter, nameof(exceptionFormatter));
        var result = new StringBuilder();
        var buffer = exception;
        while (buffer is not null)
        {
            result = result.AppendLine(exceptionFormatter(buffer)).AppendLine();
            buffer = buffer.InnerException;
        }
        return result.ToString();
    }
}