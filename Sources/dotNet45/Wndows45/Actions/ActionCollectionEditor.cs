using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Mohammad.Win.Actions
{
    internal sealed class ActionCollectionEditor : CollectionEditor
    {
        private Type[] returnedTypes;

        public ActionCollectionEditor()
            : base(typeof(ActionCollection)) { }

        protected override Type[] CreateNewItemTypes() { return this.returnedTypes; }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (this.returnedTypes == null)
                this.returnedTypes = GetReturnedTypes(provider);
            return base.EditValue(context, provider, value);
        }

        private static Type[] GetReturnedTypes(IServiceProvider provider)
        {
            var res = new List<Type>();

            var tds = (ITypeDiscoveryService) provider.GetService(typeof(ITypeDiscoveryService));

            if (tds != null)
                foreach (Type actionType in tds.GetTypes(typeof(Action), false))
                    if (actionType.GetCustomAttributes(typeof(StandardActionAttribute), false).Length > 0 && !res.Contains(actionType))
                        res.Add(actionType);

            return res.ToArray();
        }
    }
}