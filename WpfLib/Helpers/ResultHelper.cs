using Library.Results;
using Library.Wpf.Dialogs;

namespace Library.Wpf.Helpers;
public static class ResultHelper
{
    public static MsgBoxMessage ToMsgBoxMessage(this ResultBase model!!)
    {
        var result = new MsgBoxMessage();
        if (!model.Message.IsNullOrEmpty())
        {
            result.InstructionText = model.Message;
        }
        if (model.Message.IsNullOrEmpty() && model.Errors.Count == 1)
        {
            result.Text = model.Errors[0].Message?.ToString() ?? "An error occurred.";
        }
        else
        {
            var buffer = new StringBuilder();
            foreach (var errorMessage in model.Errors.Select(x => x.Message?.ToString()).Compact())
            {
                buffer.AppendLine($"- {errorMessage}");
            }
            result.Text = buffer.ToString();
        }
        return result;
    }
}
