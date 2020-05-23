using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Mohammad.Helpers;

namespace Mohammad.Wpf.Windows.Input.LibCommands
{
    public class LibCommandManager : ILibCommandManager
    {
        private bool _IsEnabled = true;
        private Visibility _Visibility = Visibility.Visible;
        protected List<LibCommand> Commands { get; } = new List<LibCommand>();

        public LibCommand this[string commandName]
        {
            get { return this.Commands.FirstOrDefault(cmd => cmd.CommandName == commandName); }
        }

        public bool IsEnabled
        {
            get => this._IsEnabled;
            set
            {
                if (value.Equals(this._IsEnabled))
                {
                    return;
                }

                this._IsEnabled = value;
                foreach (var command in this.Commands)
                {
                    command.IsEnabled = value;
                }

                this.OnPropertyChanged();
            }
        }

        public Visibility Visibility
        {
            get => this._Visibility;
            set
            {
                if (value.Equals(this._Visibility))
                {
                    return;
                }

                this._Visibility = value;
                foreach (var command in this.Commands)
                {
                    command.Visibility = value;
                }

                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerator<LibCommand> GetEnumerator() => this.Commands.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void Add(LibCommand command)
        {
            this.Commands.Add(command);
        }

        public void Remove(LibCommand command)
        {
            this.Commands.Remove(command);
        }

        public void Add(params LibCommand[] commands)
        {
            this.Commands.AddRange(commands);
            foreach (var command in this.Commands)
            {
                command.IsEnabled = this.IsEnabled;
            }
        }

        public void Add(IEnumerable<LibCommand> commands)
        {
            this.Add(commands.ToArray());
        }

        public static void Initialize(UIElement parent, params LibCommand[] commands)
        {
            foreach (var command in commands)
            {
                command.Initialize(parent);
            }
        }

        public static void Initialize(UIElement parent, Type commandsStaticClassType)
        {
            var fields = commandsStaticClassType.GetFields();
            foreach (var field in fields)
            {
                field.GetValue(null).As<LibCommand>().Initialize(parent);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged.Raise(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}