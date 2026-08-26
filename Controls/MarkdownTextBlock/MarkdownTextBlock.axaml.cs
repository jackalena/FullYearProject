using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.VisualTree;
using CSharpMath.Avalonia;
using LiveMarkdown.Avalonia;

namespace FullYearProject.Controls.MarkdownTextBlock;

/// <summary>
///     A text block that renders Markdown, including LaTeX equations.
/// </summary>
public partial class MarkdownTextBlock : UserControl
{
    /// <summary>
    ///     Identifies the <see cref="Text" /> dependency property.
    /// </summary>
    public static readonly StyledProperty<string?> TextProperty = TextBox.TextProperty.AddOwner<MarkdownTextBlock>();

    /// <summary>
    ///     Initializes a new instance of the <see cref="MarkdownTextBlock" /> class.
    /// </summary>
    public MarkdownTextBlock()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Gets or sets the Markdown text to show.
    /// </summary>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    ///     Called when an Avalonia property changes value.
    /// </summary>
    /// <param name="e">An <see cref="AvaloniaPropertyChangedEventArgs" /> object describing the property change.</param>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == TextProperty)
        {
            var text = Text;

            ObservableStringBuilder builder;

            // If we haven't created a builder yet and text needs to be shown, create one, otherwise use the existing one.
            if (MarkdownRenderer.MarkdownBuilder == null)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                MarkdownRenderer.MarkdownBuilder = builder = new();
            }
            else
            {
                builder = MarkdownRenderer.MarkdownBuilder;
            }

            // Update the builder with the new text.
            builder.Clear();
            builder.Append(text);
        }
    }

    /// <inheritdoc />
    protected override Size ArrangeOverride(Size finalSize)
    {
        // If the text changes, Arrange will be called on the control, so we can use this to apply the LaTeX equation colour fix.
        Dispatcher.Post(ApplyTextColour);

        return base.ArrangeOverride(finalSize);
    }

    // Function to apply the correct foreground colour to LaTeX equations, which would otherwise be black, since LiveMarkdown.Avalonia.Math doesn't
    // change the text colour of equations to match the text foreground colour.
    private void ApplyTextColour()
    {
        // Find the text block that contains the Markdown.
        var textBlock = MarkdownRenderer.FindDescendantOfType<LiveMarkdown.Avalonia.MarkdownTextBlock>();

        if (textBlock == null)
        {
            return;
        }

        // Find any MathViews inside the text block and set their foreground colour to the current foreground colour.
        foreach (var child in textBlock.GetLogicalChildren())
        {
            if (child is InlineUIContainer { Child: Panel panel })
            {
                foreach (var panelChild in panel.Children)
                {
                    if (panelChild is MathView mathView)
                    {
                        mathView.TextColor = ((ISolidColorBrush?)Foreground)?.Color ?? Colors.Black;
                    }
                }
            }
        }
    }
}