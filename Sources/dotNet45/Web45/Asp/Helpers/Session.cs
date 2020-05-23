using System.Dynamic;
using System.Web;

namespace Mohammad.Web.Asp.Helpers {
    public class Session : DynamicObject
    {
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = HttpContext.Current.Session[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            HttpContext.Current.Session[binder.Name] = value;
            return true;
        }
    }
}