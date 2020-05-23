using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mohammad.Wpf.Helpers;
using Mohammad.Wpf.Windows.Input.LibCommands;

namespace Mohammad.Wpf.Windows.Controls
{
    /// <summary>
    ///     Interaction logic for CommandBar.xaml
    /// </summary>
    public partial class CommandBar
    {
        private readonly LibCommandManager _CommandManager = new LibCommandManager();
        public Style ButtonsStyle { get; set; }
        public List<LibCommand> AppCommands { get; } = new List<LibCommand>();
        public List<LibCommand> PageCommands { get; } = new List<LibCommand>();
        public ILibCommandManager CommandManager => this._CommandManager;
        public CommandBar() { this.InitializeComponent(); }

        public LibCommand GetCommandByCommandName(string commandName) => this.Commands.FirstOrDefault(cmd => cmd.CommandName == commandName);

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ControlHelper.IsDesignTime())
                return;
            var appCommands = this.AppCommands;
            var pageCommands = this.PageCommands;

            this.AddCommands(appCommands, pageCommands);

            this._CommandManager.Add(this.Commands);
        }

        internal void AddCommands(IEnumerable<LibCommand> appCommands, IEnumerable<LibCommand> pageCommands)
        {
            foreach (var command in appCommands)
            {
                var button = new Button();
                if (this.ButtonsStyle != null)
                    button.Style = this.ButtonsStyle;
                command.Initialize(this, button);
                this.AppCommandPanel.Children.Add(button);
            }

            foreach (var command in pageCommands)
            {
                var button = new Button();
                if (this.ButtonsStyle != null)
                    button.Style = this.ButtonsStyle;
                command.Initialize(this, button);
                this.PageCommandPanel.Children.Add(button);
            }
        }
    }
}