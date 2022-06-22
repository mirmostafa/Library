using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using Library.CodeGeneration.Models;
using Library.Wpf.Bases;

namespace Library.Wpf.Controls;
/// <summary>
/// Interaction logic for SourceCodeTextBox.xaml
/// </summary>
public partial class SourceCodeTextBox : UserControl
{
    public string? CodeInRtf
    {
        get => (string?)this.GetValue(CodeInRtfProperty);
        set => this.SetValue(CodeInRtfProperty, value);
    }

    public static readonly DependencyProperty CodeInRtfProperty = ControlHelper.GetDependencyProperty<string?, SourceCodeTextBox>(nameof(CodeInRtf));

    public SourceCodeTextBox()
    {
        this.InitializeComponent();
        DataContextChanged += this.SourceCodeTextBox_DataContextChanged;
    }

    private void SourceCodeTextBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is SourceCodeTextBoxViewModel viewModel)
        {
            if (viewModel.Code.Language == Languages.CSharp)
            {
                _ = this.CodeStatementRichTextBox.InsertCSharpCodeToDocument(viewModel.Code);
            }
            else
            {
                var para = new Paragraph();
                para.Inlines.Add(new Run(viewModel.Code.Statement));
                this.CodeStatementRichTextBox.Document.Blocks.Add(para);
            }
        }
    }
}
public sealed record SourceCodeTextBoxViewModel(Code Code) : IViewModel;

public sealed class CodeToSourceCodeTextBoxViewModelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value switch
    {
        Code code => new SourceCodeTextBoxViewModel(code),
        SourceCodeTextBoxViewModel viewModel => viewModel,
        _ => throw new NotSupportedException()
    };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value switch
    {
        SourceCodeTextBoxViewModel viewModel => viewModel.Code,
        Code code => code,
        _ => throw new NotSupportedException()
    };
}
