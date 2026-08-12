using System.Diagnostics;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Markup.Xaml;

namespace FullYearProject.Controls;

public partial class MarkdownTextBlock : UserControl
{
    public static readonly StyledProperty<string?> TextProperty = TextBox.TextProperty.AddOwner<MarkdownTextBlock>();

    public string? Text {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public MarkdownTextBlock()
    {
        InitializeComponent();

        MarkdownRenderer.MarkdownBuilder = new();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);


        if(e.Property == TextProperty)
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