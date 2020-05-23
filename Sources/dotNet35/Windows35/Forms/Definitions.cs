#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:01 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library35.Windows.Forms
{
	public enum MsgBoxExDialogResult
	{
		None,
		OK,
		Cancel,
		Close,
		Yes,
		No,
		YesToAll,
		NoToAll,
		Abort,
		Retry,
		Ignore,
		Continue
	}

	public enum MsgBoxExDialogDefaultButton
	{
		None,
		Button1,
		Button2,
		Button3,
		Button4,
		Button5,
		Button6,
		Button7,
		Button8,
		Button9,
		Button10,
		Button11
	}

	public enum MsgBoxExDialogIcon
	{
		None,
		Information,
		Question,
		Warning,
		Error,
		SecuritySuccess,
		SecurityQuestion,
		SecurityWarning,
		SecurityError,
		SecurityShield,
		SecurityShieldBlue,
		SecurityShieldGray
	}

	public interface ISound
	{
		void Play();
	}
}