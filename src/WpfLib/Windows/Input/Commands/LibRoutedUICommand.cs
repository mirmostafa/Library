using System.Windows.Input;

using Library.EventsArgs;

namespace Library.Wpf.Windows.Input.Commands;

public class LibRoutedUICommand : RoutedUICommand, ILibCommand
{
    public LibRoutedUICommand()
        : base()
    {
    }

    public LibRoutedUICommand(string text, string name, Type ownerType)
        : base(text, name, ownerType)
    {
    }

    public LibRoutedUICommand(string text, string name, Type ownerType, InputGestureCollection inputGestures)
        : base(text, name, ownerType, inputGestures)
    {
    }

    public bool IsEnabled { get; set; }
}

public class NavigationUICommand : LibRoutedUICommand
{
    public event EventHandler<ItemActedEventArgs<NavigatingEventArgs>>? Navigating;

    public NavigationUICommand()
            : base() => this.InitializeComponents();

    public NavigationUICommand(string text, string name, Type ownerType)
        : base(text, name, ownerType) => this.InitializeComponents();

    public NavigationUICommand(string text, string name, Type ownerType, InputGestureCollection inputGestures)
        : base(text, name, ownerType, inputGestures) => this.InitializeComponents();

    public NavigationUICommand(Frame? frame, Uri? source)
    {
        (this.Frame, this.Source) = (frame, source);
        this.InitializeComponents();
    }

    public NavigationUICommand(string text, string name, Type ownerType, Frame? frame, Uri? source)
        : base(text, name, ownerType)
    {
        (this.Frame, this.Source) = (frame, source);
        this.InitializeComponents();
    }

    public NavigationUICommand(string text, string name, Type ownerType, InputGestureCollection inputGestures, Frame? frame, Uri? source)
        : base(text, name, ownerType, inputGestures)
    {
        (this.Frame, this.Source) = (frame, source);
        this.InitializeComponents();
    }

    public Frame? Frame { get; set; }

    public Uri? Source { get; set; }

    public bool Navigate()
    {
        if (!this.IsEnabled)
        {
            return false;
        }

        Check.MustBeNotNull(this.Frame);
        Check.MustBeNotNull(this.Source);

        return this.Frame.Navigate(this.Source);
    }

    public bool Navigate(Frame frame, Uri source)
    {
        (this.Frame, this.Source) = (frame, source);
        return this.Navigate();
    }

    protected virtual void OnNavigating(ItemActedEventArgs<NavigatingEventArgs> args)
    {
        if (Navigating is not null)
        {
            Navigating(this, args);
        }
        else
        {
            _ = args.Item.Frame is not null && args.Item.Source is not null
                ? this.Navigate(args.Item.Frame, args.Item.Source)
                : this.Navigate();
        }
    }

    private void InitializeComponents() 
        => this.IsEnabled = true;

    public record NavigatingEventArgs(Frame? Frame, Uri? Source);
}