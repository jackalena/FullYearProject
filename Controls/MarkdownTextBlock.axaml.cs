using Avalonia;
using Avalonia.Controls;

namespace FullYearProject.Controls;

public partial class MarkdownTextBlock : UserControl
{
    public static readonly StyledProperty<string?> TextProperty = TextBox.TextProperty.AddOwner<MarkdownTextBlock>();

    public MarkdownTextBlock()
    {
        InitializeComponent();

        App.EnsureMarkdownInitialised();
        MarkdownRenderer.MarkdownBuilder = new();
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == TextProperty)
        {
            var builder = MarkdownRenderer.MarkdownBuilder;

            if (builder == null)
            {
                return;
            }

            builder.Clear();
            builder.Append(Text);
        }
    }
}