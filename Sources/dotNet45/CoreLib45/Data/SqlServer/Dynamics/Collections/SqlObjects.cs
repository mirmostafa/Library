﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mohammad.Helpers;

namespace Mohammad.Data.SqlServer.Dynamics.Collections
{
    public abstract class SqlObjects<TSqlObject> : IEnumerable<TSqlObject>
        where TSqlObject : class, ISqlObject
    {
        private readonly Lazy<IEnumerable<TSqlObject>> _Items;

        protected SqlObjects(IEnumerable<TSqlObject> items) => this._Items = new Lazy<IEnumerable<TSqlObject>>(() => items);

        protected SqlObjects(Func<IEnumerable<TSqlObject>> itemsCreator) => this._Items = new Lazy<IEnumerable<TSqlObject>>(itemsCreator);

        public virtual TSqlObject this[int index] => this.Items.ElementAt(index);
        public TSqlObject this[string name] => this.Items.FirstOrDefault(item => item.Name.EqualsTo(name));
        protected IEnumerable<TSqlObject> Items => this._Items.Value;
        public IEnumerator<TSqlObject> GetEnumerator() => this.Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}