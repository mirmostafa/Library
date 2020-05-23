using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml.Linq;

namespace Mohammad.Dynamic.Xml
{
    public sealed class XNodeList : DynamicObject, IEnumerable
    {
        private readonly List<XElement> _Elements;
        public XNodeList(IEnumerable<XElement> elements) { this._Elements = new List<XElement>(elements); }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            var targetType = binder.Type;
            if (targetType != typeof(IEnumerable))
                return base.TryConvert(binder, out result);
            result = this;
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length != 1)
                return base.TryGetIndex(binder, indexes, out result);
            result = new XNode(this._Elements[Convert.ToInt32(indexes[0])]);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (string.Compare("Length", binder.Name, StringComparison.Ordinal) != 0)
                return base.TryGetMember(binder, out result);
            result = this._Elements.Count;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => new NodeEnumerator(this._Elements.GetEnumerator());

        private sealed class NodeEnumerator : IEnumerator
        {
            private readonly IEnumerator<XElement> _ElementEnumerator;
            public NodeEnumerator(IEnumerator<XElement> elementEnumerator) { this._ElementEnumerator = elementEnumerator; }

            public object Current
            {
                get
                {
                    var element = this._ElementEnumerator.Current;
                    return new XNode(element);
                }
            }

            public bool MoveNext() => this._ElementEnumerator.MoveNext();

            public void Reset() { this._ElementEnumerator.Reset(); }
        }
    }
}