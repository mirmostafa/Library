#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

using System.Collections.Generic;
using System.Linq;
using Mohammad.Dynamic;

namespace Mohammad.Helpers.Html
{
    public class HtmlTag : Expando
    {
        internal HtmlTag(string name) => this.Name = name;

        public IEnumerable<KeyValuePair<string, string>> Attributes
        {
            get { return this.Properties.Select(p => new KeyValuePair<string, string>(p.Key, (string) (p.Value ?? string.Empty))); }
        }

        public string Name          { get; }
        public bool   TrailingSlash { get; internal set; }

        internal void Add(string attribute, string value)
        {
            this.Properties.Add(attribute, value);
        }

        internal bool Contains(string attribute) => this.Properties.ContainsKey(attribute);

        internal void Remove(string attribute)
        {
            this.Properties.Remove(attribute);
        }
    }
}