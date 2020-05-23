namespace Mohammad.Win.Forms
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