using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Mohammad.Wpf.Windows.Input.LibCommands;

namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    public class BaseTile : LibUserControl
    {
        private int _BadgeFontSize = 12;
        private int _BodyFontSize;
        private LibCommand _Command;
        private Effect _CurrentEffect;
        private int _HeadingFontSize;
        private TileScale _Scale;

        public int HeadingFontSize
        {
            get => this._HeadingFontSize;
            set
            {
                if (value == this._HeadingFontSize)
                    return;
                this._HeadingFontSize = value;
                this.OnPropertyChanged();
            }
        }

        public int BodyFontSize
        {
            get => this._BodyFontSize;
            set
            {
                if (value == this._BodyFontSize)
                    return;
                this._BodyFontSize = value;
                this.OnPropertyChanged();
            }
        }

        public int BadgeFontSize
        {
            get => this._BadgeFontSize;
            set
            {
                if (value == this._BadgeFontSize)
                    return;
                this._BadgeFontSize = value;
                this.OnPropertyChanged();
            }
        }

        public TileScale Scale
        {
            get => this._Scale;
            set
            {
                this._Scale = value;
                switch (value)
                {
                    case TileScale.Tile50Square:
                        this.HeadingFontSize = DefaultHeadingFontSize * 85 / 100; // 50% is too small to see.
                        this.BodyFontSize = DefaultBodyFontSize * 85 / 100;
                        this.Width = 75;
                        this.Height = 75;
                        break;
                    case TileScale.Tile50Wide:
                        this.HeadingFontSize = DefaultHeadingFontSize * 85 / 100; // 50% is too small to see.
                        this.BodyFontSize = DefaultBodyFontSize * 85 / 100;
                        this.Width = 155;
                        this.Height = 75;
                        break;
                    case TileScale.Tile80Square:
                        //this.HeadingFontSize = DefaultHeadingFontSize * 95 / 100;
                        //this.BodyFontSize = DefaultBodyFontSize * 95 / 100;
                        this.Width = 120;
                        this.Height = 120;
                        break;
                    case TileScale.Tile80Wide:
                        //this.HeadingFontSize = DefaultHeadingFontSize * 95 / 100;
                        //this.BodyFontSize = DefaultBodyFontSize * 95 / 100;
                        this.Width = 248;
                        this.Height = 120;
                        break;
                    case TileScale.Tile100Square:
                        this.HeadingFontSize = DefaultHeadingFontSize * 100 / 100;
                        this.BodyFontSize = DefaultBodyFontSize * 100 / 100;
                        this.Width = 150;
                        this.Height = 150;
                        break;
                    case TileScale.Tile100Wide:
                        this.HeadingFontSize = DefaultHeadingFontSize * 100 / 100;
                        this.BodyFontSize = DefaultBodyFontSize * 100 / 100;
                        this.Width = 310;
                        this.Height = 150;
                        break;
                    case TileScale.Tile140Square:
                        this.HeadingFontSize = DefaultHeadingFontSize * 140 / 100;
                        this.BodyFontSize = DefaultBodyFontSize * 140 / 100;
                        this.Width = 210;
                        this.Height = 210;
                        break;
                    case TileScale.Tile140Wide:
                        this.HeadingFontSize = DefaultHeadingFontSize * 140 / 100;
                        this.BodyFontSize = DefaultBodyFontSize * 140 / 100;
                        this.Width = 434;
                        this.Height = 210;
                        break;
                    case TileScale.Tile180Square:
                        this.HeadingFontSize = DefaultHeadingFontSize * 180 / 100;
                        this.BodyFontSize = DefaultBodyFontSize * 140 / 180;
                        this.Width = 270;
                        this.Height = 270;
                        break;
                    case TileScale.Tile180Wide:
                        this.HeadingFontSize = DefaultHeadingFontSize * 180 / 100;
                        this.BodyFontSize = DefaultBodyFontSize * 140 / 180;
                        this.Width = 558;
                        this.Height = 270;
                        break;
                    case TileScale.Tile360Wide:
                        this.HeadingFontSize = DefaultHeadingFontSize * 180 / 100;
                        this.BodyFontSize = DefaultBodyFontSize * 140 / 180;
                        this.Width = 558;
                        this.Height = 75;
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(value));
                }
                this.OnPropertyChanged();
            }
        }

        public static int DefaultHeadingFontSize { get; set; } = 26;
        public static int DefaultBodyFontSize { get; set; } = 14;

        public LibCommand Command
        {
            get => this._Command;
            set
            {
                if (this._Command == value)
                    return;
                this._Command = value;
                var libCommand = this._Command;
                if (libCommand != null)
                    libCommand.PropertyChanged += this.Command_OnPropertyChanged;
                this.OnCommandChanged();
            }
        }

        private void Command_OnPropertyChanged(object sender, PropertyChangedEventArgs e) { this.OnCommandChanged(); }

        protected virtual void OnCommandChanged()
        {
            var libCommand = this._Command;
            if (libCommand != null)
            {
                this.IsEnabled = libCommand.IsEnabled;
                this.ToolTip = this.Command.ToolTip;
                this.Visibility = this.Command.Visibility;
            }
            this.HookCommand();
        }

        protected virtual void HookCommand() { }
        public event EventHandler Click;

        protected virtual void OnClick()
        {
            var handler = this.Click;
            handler?.Invoke(this, EventArgs.Empty);
            this.Command?.Execute(null, this);
        }

        protected override void OnInitialized(EventArgs e)
        {
            this.Background = new LinearGradientBrush(Colors.RoyalBlue, Colors.Blue, 45);
            this.Foreground = Brushes.Snow;
            base.OnInitialized(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            this._CurrentEffect = this.Effect;
            var shadow = new DropShadowEffect {Color = Colors.Gray};
            this.Effect = shadow;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.Effect = this._CurrentEffect;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var shadow = new BlurEffect();
            this.Effect = shadow;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            var shadow = new DropShadowEffect {Color = Colors.Gray};
            this.Effect = shadow;
            base.OnMouseUp(e);
            this.OnClick();
        }
    }

    public enum TileScale
    {
        Tile50Square,
        Tile50Wide,
        Tile80Square,
        Tile80Wide,
        Tile100Square,
        Tile100Wide,
        Tile140Square,
        Tile140Wide,
        Tile180Square,
        Tile180Wide,
        Tile360Wide
    }
}