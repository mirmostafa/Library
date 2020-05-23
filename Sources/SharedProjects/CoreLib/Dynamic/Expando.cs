using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Mohammad.Properties;

namespace Mohammad.Dynamic
{
    [Serializable]
    public class Expando : DynamicObject, ISerializable, INotifyPropertyChanged
    {
        protected readonly Dictionary<string, object> Properties = new Dictionary<string, object>();

        public virtual object this[string index]
        {
            get { return this.Properties.ContainsKey(index) ? this.Properties[index] : null; }
            set
            {
                if (this.Properties.ContainsKey(index))
                {
                    if (this.Properties[index] == value)
                        return;
                    this.Properties[index] = value;
                }
                else
                {
                    this.Properties.Add(index, value);
                }
                this.OnPropertyChanged(index);
            }
        }

        public Expando() { }

        protected Expando(SerializationInfo info, StreamingContext context)
        {
            this.Properties = (Dictionary<string, object>) info.GetValue("Properties", typeof(Dictionary<string, object>));
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected virtual void GetProperties(SerializationInfo info, StreamingContext context)
        {
            foreach (var kvp in this.Properties)
                info.AddValue(kvp.Key, kvp.Value);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            this.GetProperties(info, context);
        }
    }
}