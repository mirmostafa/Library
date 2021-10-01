using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using Library.Validations;
using Library.Wpf.Commands;

namespace Library.Wpf.Windows.Input.Commands
{
    public class CommandDependencyPropertyBag<TUiElement>
        where TUiElement : UIElement
    {

        private readonly Dictionary<string, LibRoutedUICommand> _Bag = new();


        public void AddCommand(LibRoutedUICommand command, string? key = null)
        {
            Check.IfArgumentNotNull(command, nameof(command));
            this._Bag.Add(key ?? command.Name, command);
        }

        public LibRoutedUICommand this[string key]
        {
            get => this._Bag[key];
            set => this._Bag[key] = value;
        }

        public DependencyProperty GetByKey(string key, Func<bool>? canExcute, Action execute) =>
            //var command = new LibRoutedUICommand()
            //AddCommand()
            DependencyProperty.Register(
                               key,
                               typeof(LibRoutedUICommand),
                               typeof(TUiElement),
                               new FrameworkPropertyMetadata(
                                   default,
                                   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                                   null)
                               { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
                               value => this[key].CanExecute(null, null));

        public DependencyProperty GetByKey(string key) => DependencyProperty.Register(
                               key,
                               typeof(LibRoutedUICommand),
                               typeof(TUiElement),
                               new FrameworkPropertyMetadata(
                                   default,
                                   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                                   null)
                               { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
                               value => this[key].CanExecute(null, null));
    }
}
