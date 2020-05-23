#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml.Linq;

namespace Library40.Dynamic.Xml
{
	public sealed class XNodeList : DynamicObject, IEnumerable
	{
		private readonly List<XElement> _Elements;

		public XNodeList(IEnumerable<XElement> elements)
		{
			this._Elements = new List<XElement>(elements);
		}

		public override bool TryConvert(ConvertBinder binder, out object result)
		{
			var targetType = binder.Type;
			if (targetType == typeof (IEnumerable))
			{
				result = this;
				return true;
			}
			return base.TryConvert(binder, out result);
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			if (indexes.Length == 1)
			{
				var element = this._Elements[Convert.ToInt32(indexes[0])];
				result = new XNode(element);
				return true;
			}

			return base.TryGetIndex(binder, indexes, out result);
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (String.Compare("Length", binder.Name, StringComparison.Ordinal) == 0)
			{
				result = this._Elements.Count;
				return true;
			}

			return base.TryGetMember(binder, out result);
		}

		#region Implementation of IEnumerable
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new NodeEnumerator(this._Elements.GetEnumerator());
		}
		#endregion

		#region Nested type: NodeEnumerator
		private sealed class NodeEnumerator : IEnumerator
		{
			private readonly IEnumerator<XElement> _ElementEnumerator;

			public NodeEnumerator(IEnumerator<XElement> elementEnumerator)
			{
				this._ElementEnumerator = elementEnumerator;
			}

			#region IEnumerator Members
			public object Current
			{
				get
				{
					var element = this._ElementEnumerator.Current;
					return new XNode(element);
				}
			}

			public bool MoveNext()
			{
				return this._ElementEnumerator.MoveNext();
			}

			public void Reset()
			{
				this._ElementEnumerator.Reset();
			}
			#endregion
		}
		#endregion
	}
}