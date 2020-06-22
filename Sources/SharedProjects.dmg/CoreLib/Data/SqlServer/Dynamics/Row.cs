using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using Mohammad.Helpers;

namespace Mohammad.Data.SqlServer.Dynamics
{
    public class Row : SqlObject<Row, Table>
    {
        private IEnumerable<KeyValuePair<string, object>> Data { get; }

        public object this[string colName]
        {
            get
            {
                var q = from pair in this.Data
                        where pair.Key.EqualsTo(colName)
                        select pair;
                if (!q.Any())
                    throw new Mohammad.Exceptions.ObjectNotFoundException("Column not found.");
                return q.First().Value;
            }
        }

        public Row(Table owner, IEnumerable<KeyValuePair<string, object>> data, string connectionstring)
            : base(owner, string.Empty, connectionstring) { this.Data = data; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }
    }
}