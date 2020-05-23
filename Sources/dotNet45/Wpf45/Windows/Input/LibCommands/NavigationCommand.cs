using System;
using System.Windows;
using Mohammad.EventsArgs;
using Mohammad.Helpers;
using Mohammad.Wpf.Interfaces;
using Mohammad.Wpf.Windows.Controls;

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    public class NavigationCommand : LibCommand, IWindowHosted
    {
        private LibFrame _Frame;
        private object _NavigationContent;
        private Uri _Source;

        public LibFrame Frame
        {
            get
            {
                if (this._Frame != null)
                {
                    return this._Frame;
                }

                var e = new ItemActedEventArgs<LibFrame>(null);
                this.OnFrameRequired(e);
                this._Frame = e.Item;
                return this._Frame;
            }
            set
            {
                this._Frame = value;
                this.OnPropertyChanged();
            }
        }

        public Uri Source
        {
            get => this._Source;
            set
            {
                this._Source = value;
                this.NavigationContent = null;
                this.OnPropertyChanged();
            }
        }

        public object NavigationContent
        {
            get => this._NavigationContent;
            set
            {
                this._NavigationContent = value;
                this.OnPropertyChanged();
            }
        }

        public bool CanPackSource { get; set; } = true;
        public Window Window { get; set; }
        public bool Navigate(Uri source) => this.CanExecute() && this.Frame.Navigate(source);
        public bool Navigate(Uri source, object extraData) => this.CanExecute() && this.Frame.Navigate(source, extraData);
        public bool Navigate(object content) => this.CanExecute() && this.Frame.Navigate(content);
        public bool Navigate(object content, object extraData) => this.CanExecute() && this.Frame.Navigate(content, extraData);

        protected override void OnExecuted()
        {
            if (!this.CanExecute())
            {
                return;
            }

            this.Frame.Window = this.Window;
            var args = new ItemActingEventArgs<object>();
            this.OnNavigating(args);
            if (args.Item != null)
            {
                this.Navigate(args.Item);
            }
            else if (this.Source != null)
            {
                if (this.CanPackSource)
                {
                    this.Frame.Source = this.Source.ToString().StartsWith("pack://application:,,,/")
                        ? this.Source
                        : this.Source.ToString().StartsWith("/")
                            ? new Uri("pack://application:,,," + this.Source)
                            : new Uri("pack://application:,,,/" + this.Source);
                }
                else
                {
                    this.Frame.Source = this.Source;
                }
            }
            else if (this.NavigationContent != null)
            {
                this.Navigate(this.NavigationContent);
            }

            this.NavigationContent = this.Frame.Content;
            base.OnExecuted();
        }

        protected virtual void OnNavigating(ItemActingEventArgs<object> args)
        {
            this.Navigating.Raise(this, args);
        }

        protected override bool OnCanExecute() => this.Frame != null && base.OnCanExecute();

        protected virtual void OnFrameRequired(ItemActedEventArgs<LibFrame> e)
        {
            this.FrameRequired?.Invoke(this, e);
        }

        public event EventHandler<ItemActingEventArgs<object>> Navigating;
        public event EventHandler<ItemActedEventArgs<LibFrame>> FrameRequired;
    }
}