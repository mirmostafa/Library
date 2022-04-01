using System.Windows.Input;

namespace Library.Wpf.Windows.Input.Commands;

public static class LibCommonCommands
{
    public static readonly LibRoutedUICommand New = new(
            "New",
            "NewCommand",
            typeof(LibCommonCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
        );

    public static readonly LibRoutedUICommand Edit = new(
            "Edit",
            "EditCommand",
            typeof(LibCommonCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Control)
            }
        );

    public static readonly LibRoutedUICommand Delete = new(
           "Delete",
           "DeleteCommand",
           typeof(LibCommonCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.Delete, ModifierKeys.Control)
           }
       );
    public static readonly LibRoutedUICommand Undo = new(
           "Undo",
           "UndoCommand",
           typeof(LibCommonCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.Z, ModifierKeys.Control),
           }
       );
    public static readonly LibRoutedUICommand Save = new(
           "Save",
           "SaveCommand",
           typeof(LibCommonCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.S, ModifierKeys.Control)
           }
       );
    public static readonly LibRoutedUICommand Reset = new(
           "Reset",
           "ResetCommand",
           typeof(LibCommonCommands)
       );

    public static readonly LibRoutedUICommand Validate = new(
           "Validate",
           "ValidateCommand",
           typeof(LibCommonCommands)
       );
}
