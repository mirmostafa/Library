using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Mohammad.Helpers;
using Mohammad.Wpf.EventsArgs;
using Mohammad.Wpf.Interfaces;

namespace Mohammad.Wpf.Windows.Controls
{
    public class PageBase : Page, INotifyPropertyChanged, IWindowHosted
    {
        public static readonly DependencyProperty WindowProperty = DependencyProperty.Register("Window",
            typeof(Window),
            typeof(PageBase),
            new PropertyMetadata(default(Window),
                (sender, __) =>
                {
                    var me = sender.As<PageBase>();
                    me.OnWindowChanged();
                    me.OnPropertyChanged();
                }));

        private bool _IsFirstDataRebind = true;

        public Window Window
        {
            get => (Window)this.GetValue(WindowProperty);
            set => this.SetValue(WindowProperty, value);
        }

        public PageBase()
            : this(null)
        {
        }

        protected PageBase(Window window)
        {
            this.Window = window ?? Application.Current.MainWindow;

            this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (ThreadStart)(() => this.OnApplicationIdle(this)));
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (ThreadStart)(() => this.OnBackground(this)));
            //this.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, (ThreadStart) (() => this.OnDataBind(this)));
            this.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, (ThreadStart)this.BindData);
            this.Dispatcher.BeginInvoke(DispatcherPriority.Input, (ThreadStart)(() => this.OnInput(this)));
            this.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart)(() => this.OnSend(this)));
            this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, (ThreadStart)(() => this.OnSystemIdle(this)));
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        public void BindData()
        {
            this.OnBindingData(new BindingDataEventArgs(this._IsFirstDataRebind));
            this._IsFirstDataRebind = false;
        }

        /// <summary>
        ///     Operations are processed when the application is idle.
        /// </summary>
        /// <param name="senderPageBase"></param>
        protected virtual void OnApplicationIdle(PageBase senderPageBase)
        {
        }

        /// <summary>
        ///     Operations are processed after all other non-idle operations are completed.
        /// </summary>
        /// <param name="senderPageBase"></param>
        protected virtual void OnBackground(PageBase senderPageBase)
        {
        }

        ///// <summary>
        /////     Operations are processed at the same priority as data binding.
        ///// </summary>
        ///// <param name="senderPageBase"></param>
        //protected virtual void OnDataBind(PageBase senderPageBase)
        //{}

        /// <summary>
        ///     Operations are processed at the same priority as input.
        /// </summary>
        /// <param name="senderPageBase"></param>
        protected virtual void OnInput(PageBase senderPageBase)
        {
        }

        /// <summary>
        ///     Operations are processed before other asynchronous operations. This is the highest priority.
        /// </summary>
        /// <param name="senderPageBase"></param>
        protected virtual void OnSend(PageBase senderPageBase)
        {
        }

        /// <summary>
        ///     The enumeration value is 1. Operations are processed when the system is idle.
        /// </summary>
        /// <param name="senderPageBase"></param>
        protected virtual void OnSystemIdle(PageBase senderPageBase)
        {
        }

        protected virtual void OnBindingData(BindingDataEventArgs e)
        {
            this.BindingData.Raise(this, e);
        }

        protected virtual void OnWindowChanged()
        {
            //this.MoveBarsToParent();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnTitleChanged()
        {
            this.TitleChanged.Raise(this);
        }

        public virtual event EventHandler<BindingDataEventArgs> BindingData;
        public virtual event EventHandler TitleChanged;
    }
}