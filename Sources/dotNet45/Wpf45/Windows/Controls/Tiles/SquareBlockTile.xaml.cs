namespace Mohammad.Wpf.Windows.Controls.Tiles
{
    /// <summary>
    ///     Interaction logic for SquareBlockTile.xaml
    /// </summary>
    public partial class SquareBlockTile : BaseTile
    {
        public string Header
        {
            get { return this.textBlock1.Text; }
            set
            {
                this.textBlock1.Text = value;
                //this.SetValue(HeaderProperty, value);
            }
        }

        public string Body
        {
            get { return this.textBlock2.Text; }
            set
            {
                this.textBlock2.Text = value;
                //this.SetValue(BodyProperty, value);
            }
        }

        //public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
        //	typeof (string),
        //	typeof (SquareBlockTile),
        //	new PropertyMetadata(default(string)));
        //public static readonly DependencyProperty BodyProperty = DependencyProperty.Register("Body", typeof (string), typeof (SquareBlockTile), new PropertyMetadata(default(string)));

        public SquareBlockTile() { this.InitializeComponent(); }
    }
}