using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using FullYearProject.ViewModels;
using FullYearProject.Views;

using LiveMarkdown.Avalonia;

using org.mariuszgromada.math.mxparser;

namespace FullYearProject;

/// <summary>
///     Defines the Avalonia application.
/// </summary>
public class App : Application
{
    // Flag to ensure that the Markdown renderer is only initialised once
    private static bool _hasInitialisedMarkdown;

    /// <inheritdoc />
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted()
    {
        // Initialise the math equation parser.
        License.iConfirmNonCommercialUse("Jack Ishmael");

        // Create the main window for the application.
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel() };
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    ///     Ensure that the Markdown renderer is initialised.
    /// </summary>
    public static void EnsureMarkdownInitialised()
    {
        if (_hasInitialisedMarkdown)
        {
            return;
        }

        // Register the Markdown nodes for math equations to allow LaTeX rendering.
        MarkdownNode.Register<MathInlineNode>();
        MarkdownNode.Register<MathBlockNode>();

        _hasInitialisedMarkdown = true;
    }
}