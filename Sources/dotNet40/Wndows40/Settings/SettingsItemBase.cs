#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.Win.Settings
{
	[Serializable]
	public class SettingsItemBase
	{
		public virtual void Reset()
		{
			foreach (var item in this.GetType().GetProperties())
				if (item.CanWrite)
					try
					{
						item.SetValue(this, null, null);
					}
					catch
					{
						try
						{
							item.SetValue(this, 0, null);
						}
						catch
						{
						}
					}
		}
	}
}