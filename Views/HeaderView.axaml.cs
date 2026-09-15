using Avalonia;
using Avalonia.Controls;

namespace FullYearProject.Views;

/// <summary>
///     Control to show a header with the quiz icon and title.
/// </summary>
public partial class HeaderView : UserControl
{
    /// <summary>
    ///     Identifies the <see cref="Title" /> dependency property.
    /// </summary>
    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<HeaderView, string>(nameof(Title), "Title");

    /// <summary>
    ///     The title to show in the header.
    /// </summary>
    public string Title {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="HeaderView" /> class.
    /// </summary>
    public HeaderView()
    {
        InitializeComponent();
    }
}