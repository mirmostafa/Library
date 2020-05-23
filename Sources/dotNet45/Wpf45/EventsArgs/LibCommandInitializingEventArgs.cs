using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Mohammad.Wpf.EventsArgs
{
    public class LibCommandInitializingEventArgs : EventArgs
    {
        //public LibCommandInitializingEventArgs(UIElement parent,
        //									   ExecutedRoutedEventHandler execute,
        //									   CanExecuteRoutedEventHandler canExecute = null,
        //									   KeyGesture gesture = null,
        //									   params UIElement[] elements)
        //{
        //	this.Parent = parent;
        //	this.Execute = execute;
        //	this.CanExecute = canExecute;
        //	this.Gesture = gesture;
        //	this.Elements = elements;
        //}

        public UIElement Parent { get; set; }
        public ExecutedRoutedEventHandler Execute { get; set; }
        public CanExecuteRoutedEventHandler CanExecute { get; set; }
        public KeyGesture Gesture { get; set; }
        public List<UIElement> Elements { get; } = new List<UIElement>();
    }
}