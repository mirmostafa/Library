#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;

namespace Library40.Interfaces
{
	public interface ISettingsEnabledElement
	{
		void LoadSettings();
		void SaveSettings();
		event EventHandler LoadingSettings;
		event EventHandler SavingSettings;
	}
}