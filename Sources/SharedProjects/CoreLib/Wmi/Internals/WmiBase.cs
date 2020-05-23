using System;
using System.Linq;
using System.Management;
using Mohammad.Helpers;

namespace Mohammad.Wmi.Internals
{
    public class WmiBase
    {
        public bool Initiated { get; protected set; }
        public bool IsLocal { get; }

        public WmiBase(bool initialize = true)
        {
            this.IsLocal = true;
            if (initialize)
                this.Initialize();
        }

        public virtual void Initialize()
        {
            if (this.Initiated)
                return;
            using (var searcher = this.GetSearcher())
            using (var queryObj = searcher.Get().Cast<ManagementObject>().FirstOrDefault())
                foreach (var property in this.GetType().GetProperties())
                {
                    var wmiProp = property.GetCustomAttributes(true).Cast<Attribute>().FirstOrDefault(att => att is WmiPropAttribute) as WmiPropAttribute;
                    if (wmiProp == null)
                        continue;
                    var wmiPropName = wmiProp.Name ?? property.Name;
                    if (queryObj != null)
                        property.SetValue(this, queryObj[wmiPropName], new object[] {});
                }
            this.Initiated = true;
        }

        protected virtual ManagementObjectSearcher GetSearcher()
            =>
                this.IsLocal
                    ? new ManagementObjectSearcher($"root\\{ObjectHelper.GetAttribute<WmiClassAttribute>(this).Namespace}",
                        $"SELECT * FROM {ObjectHelper.GetAttribute<WmiClassAttribute>(this).ClassName}")
                    : null;
    }
}