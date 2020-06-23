// Created on     2017/12/17
// Last update on 2018/01/03 by Mohammad Mir mostafa 

using System.Web;
using Mohammad.Helpers;
using Mohammad.Net;

namespace Mohammad.Web.Api.Tools
{
    public class ServerTools
    {
        public static IpAddress GetClientIpAddress()
        {
            var context = HttpContext.Current;
            if (context == null)
            {
                return null;
            }

            var ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            var requestServerVariable = context.Request.ServerVariables["REMOTE_ADDR"];
            if (ipAddress.IsNullOrEmpty())
            {
                return IpAddress.Parse(requestServerVariable);
            }

            var addresses = ipAddress.Split(',');
            var clientIpAddress = addresses.Length != 0 ? addresses[0] : requestServerVariable;
            return IpAddress.Parse(clientIpAddress);
        }
    }
}