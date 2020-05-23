#region Code Identifications

// Created on     2017/12/17
// Last update on 2017/12/17 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using Mohammad.Helpers;

namespace Mohammad.Web.Api.Helpers
{
    public static class ApiHelpers
    {
        public static object GetHeaderValue(this ApiController controller, string headerKey) => controller.ActionContext.GetHeaderValue(headerKey);

        public static object GetArgumentValue(this ApiController controller, string argName) => controller.ActionContext.GetArgumentValue(argName);

        public static string GetHeaderValue(this HttpActionContext context, string headerKey) => GetHeaderValues(context, headerKey)?.FirstOrDefault();

        public static IEnumerable<string> GetHeaderValues(this HttpActionContext context, string headerKey)
        {
            return context?.Request?.Headers?.FirstOrDefault(arg => arg.Key.EqualsTo(headerKey)).Value;
        }

        public static object GetArgumentValue(this HttpActionContext context, string argName)
        {
            return context.ActionArguments.FirstOrDefault(arg => arg.Key.EqualsTo(argName)).Value;
        }

        public static (string Value, bool IsFound) TryLookFor(this ApiController controller, string key) => controller.ActionContext.TryLookFor(key);

        public static (string Value, bool IsFound) TryLookFor(this HttpActionContext actionContext, string key)
        {
            var value = actionContext.GetArgumentValue(key)?.ToString();
            if (value.IsNullOrEmpty())
            {
                value = actionContext.GetHeaderValue(key);
            }

            return (value, !value.IsNullOrEmpty());
        }
    }
}