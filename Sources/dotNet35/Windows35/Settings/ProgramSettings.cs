#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:02 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library35.Serialization;

namespace Library35.Windows.Settings
{
	public abstract class ProgramSettings<TProgramSettings> : ApplicationSettings<TProgramSettings>
		where TProgramSettings : ProgramSettings<TProgramSettings>, new()
	{
	}
}