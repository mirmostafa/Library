using System;

namespace Mohammad.Web.Asp.Security
{
    public class AuthorizeAttribute : Attribute
    {
        public bool IsAdminPrivilegeRequired { get; set; }
    }
}