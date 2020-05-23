#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Library40.Collections.ObjectModel
{
	public class BindableCollection<TEntity> : ObservableCollection<TEntity>
	{
		public void AddRange(IEnumerable<TEntity> entities)
		{
			foreach (var entity in entities)
				this.Add(entity);
		}
	}
}