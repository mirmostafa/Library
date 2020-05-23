using System;
using System.Text;

namespace Mohammad.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetDeepExceptionMessage(this Exception ex)
        {
            var result = new StringBuilder();
            for (var exception = ex; exception != null; exception = exception.InnerException)
            {
                result.AppendLine(exception.Message);
                result.AppendLine("-_-_-_-_");
            }
            return result.ToString();
        }
    }
}