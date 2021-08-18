using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Library.Helpers;

namespace WpfLibrary.Windows.Input.Commands
{
    public class LibCommandBinding : CommandBinding
    {
        #region Fields

        private string _CommandText;

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        public string CommandText
        {
            get => this._CommandText;
            set
            {
                this._CommandText = value;
                this.OnPropertyChanged();
                this.OnCaptionChanged();
            }
        }

        protected virtual void OnCaptionChanged()
        {
            if (this.Command.As<RoutedUICommand>() is { } command)
            {
                command.Text = this.CommandText;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
