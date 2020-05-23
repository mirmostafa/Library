#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace Library40.Data.EntityFramework
{
	public static class ServiceBrokerUtility
	{
		private const string sqlDependencyCookie = "MS.SqlDependencyCookie";
		private static readonly List<string> _ConnectionStrings = new List<string>();
		private static ObjectContext _Ctx;
		private static RefreshMode _RefreshMode;
		private static IEnumerable _Collection;
		private static INotifyRefresh _NotifyRefresh;

		public static void AutoRefresh(this ObjectContext ctx, RefreshMode refreshMode, IEnumerable collection)
		{
			var csInEf = ctx.Connection.ConnectionString;
			var csName = csInEf.Replace("name=", "").Trim();
			var csForEf = ConfigurationManager.ConnectionStrings[csName].ConnectionString;
			var newConnectionString = new EntityConnectionStringBuilder(csForEf).ProviderConnectionString;
			if (!_ConnectionStrings.Contains(newConnectionString))
			{
				_ConnectionStrings.Add(newConnectionString);
				SqlDependency.Start(newConnectionString);
			}
			_Ctx = ctx;
			_RefreshMode = refreshMode;
			_Collection = collection;
			_NotifyRefresh = collection as INotifyRefresh;
			AutoRefresh();
		}

		public static void AutoRefresh()
		{
			var oldCookie = CallContext.GetData(sqlDependencyCookie);
			try
			{
				var dependency = new SqlDependency();
				CallContext.SetData(sqlDependencyCookie, dependency.Id);
				dependency.OnChange += delegate
				                       {
					                       AutoRefresh();
					                       if (_NotifyRefresh != null)
						                       _NotifyRefresh.OnRefresh();
				                       };
				_Ctx.Refresh(_RefreshMode, _Collection);
			}
			finally
			{
				CallContext.SetData(sqlDependencyCookie, oldCookie);
			}
		}
	}

	public class AutoRefreshWrapper<T> : IEnumerable<T>, INotifyRefresh
		where T : EntityObject
	{
		private readonly IEnumerable<T> _ObjectSet;

		public AutoRefreshWrapper(ObjectSet<T> objectSet, RefreshMode refreshMode)
		{
			this._ObjectSet = objectSet;
			objectSet.Context.AutoRefresh(refreshMode, objectSet);
		}

		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			return this._ObjectSet.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion

		#region INotifyRefresh Members
		public void OnRefresh()
		{
			if (this.CollectionChanged != null)
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;
		#endregion
	}

	public interface INotifyRefresh : INotifyCollectionChanged
	{
		void OnRefresh();
	}
}