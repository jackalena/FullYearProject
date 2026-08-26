using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FullYearProject.ViewModels;
using FullYearProject.Views;
using LiveMarkdown.Avalonia;
using org.mariuszgromada.math.mxparser;

namespace FullYearProject;

public class App : Application
{
    private static bool _hasInitialisedMarkdown;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        ///TODO: Shuffle questions
    }

    public override void OnFrameworkInitializationCompleted()
    {
        License.iConfirmNonCommercialUse("Jack Ishmael");

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel() };
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static void EnsureMarkdownInitialised()
    {
        if (_hasInitialisedMarkdown)
        {
            return;
        }

        MarkdownNode.Register<MathInlineNode>();
        MarkdownNode.Register<MathBlockNode>();

        _hasInitialisedMarkdown = true;
    }
}