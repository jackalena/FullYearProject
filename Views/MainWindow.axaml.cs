using System;
using Avalonia;
using Avalonia.Controls;

namespace FullYearProject.Views;

/// <summary>
///     Control containing the main window contents.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    ///     Initialises a new instance of the <see cref="MainWindow" /> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Event raised when the window is initialised.
    /// </summary>
    public static event Action<MainWindow>? Initialised;

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        Initialised?.Invoke(this);
    }
}