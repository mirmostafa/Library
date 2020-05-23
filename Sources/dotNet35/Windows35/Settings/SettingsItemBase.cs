#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library35.Windows.Settings
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