


using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Mohammad.Exceptions;
using Mohammad.Helpers;

namespace Mohammad.Data.SqlServer.Dynamics
{
    public class Row : SqlObject<Row, Table>
    {
        public object this[Column index] => this[index.Name];

        public object this[string colName]
        {
            get
            {
                var q = from pair in this.Data
                        where pair.Key.EqualsTo(colName)
                        select pair;
                if (!q.Any())
                {
                    throw new ObjectNotFoundException("Column not found.");
                }

                return q.First().Value;
            }
        }

        public object this[int index] => this.Data.ElementAt(index);

        private IEnumerable<KeyValuePair<string, object>> Data { get; }

        public Row(Table owner, IEnumerable<KeyValuePair<string, object>> data, string connectionString = null)
            : base(owner, string.Empty, connectionString: connectionString ?? owner.ConnectionString) => this.Data = data;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }
    }
}