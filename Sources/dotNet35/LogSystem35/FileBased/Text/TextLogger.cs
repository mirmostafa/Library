#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using Library35.LogSystem.Entities;

namespace Library35.LogSystem.FileBased.Text
{
	/// <summary>
	///     Facade for writing a log entry(es) optimized for XML log writer. This class cannot be inherited.
	/// </summary>
	public sealed class TextLogger : TextLogger<LogEntity>
	{
		public TextLogger(bool useLogRotation)
			: base(useLogRotation)
		{
		}
	}
}