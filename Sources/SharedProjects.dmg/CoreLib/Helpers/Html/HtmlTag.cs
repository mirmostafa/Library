using System.Collections.Generic;
using System.Linq;
using Mohammad.Dynamic;

namespace Mohammad.Helpers.Html
{
    public class HtmlTag : Expando
    {
        public string Name { get; private set; }
        public bool TrailingSlash { get; internal set; }

        public IEnumerable<KeyValuePair<string, string>> Attributes
        {
            get { return this.Properties.Select(p => new KeyValuePair<string, string>(p.Key, (string) (p.Value ?? string.Empty))); }
        }

        internal HtmlTag(string name) { this.Name = name; }
        internal bool Contains(string attribute) { return this.Properties.ContainsKey(attribute); }
        internal void Remove(string attribute) { this.Properties.Remove(attribute); }
        internal void Add(string attribute, string value) { this.Properties.Add(attribute, value); }
    }
}