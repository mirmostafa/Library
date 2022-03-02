using System.ComponentModel;
using System.Drawing;

namespace Library.Wpf.Controls;
/// <summary>
/// Interaction logic for LabeledControl.xaml
/// </summary>
[DefaultProperty(nameof(Client))]
public partial class LabeledControl : UserControl
{
    public Color LineColor
    {
        get => (Color)this.GetValue(LineColorProperty);
        set => this.SetValue(LineColorProperty, value);
    }
    public static readonly DependencyProperty LineColorProperty = ControlHelper.GetDependencyProperty<Color, LabeledControl>(nameof(LineColor));

    public string? Caption
    {
        get => (string?)this.GetValue(CaptionProperty);
        set => this.SetValue(CaptionProperty, value);
    }
    public static readonly DependencyProperty CaptionProperty = ControlHelper.GetDependencyProperty<string?, LabeledControl>(nameof(Caption));

    public object? Client
    {
        get => (object?)this.GetValue(ClientProperty);
        set => this.SetValue(ClientProperty, value);
    }
    public static readonly DependencyProperty ClientProperty = ControlHelper.GetDependencyProperty<object?, LabeledControl>(nameof(Client));

    public LabeledControl() => this.InitializeComponent();
}
