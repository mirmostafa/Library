#region File Notice
// Created at: 2013/12/24 3:44 PM
// Last Update time: 2013/12/24 4:03 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library40.Serialization;

namespace Library40.Win.Settings
{
	public abstract class ProgramSettings<TProgramSettings> : ApplicationSettings<TProgramSettings>
		where TProgramSettings : ProgramSettings<TProgramSettings>, new()
	{
	}
}