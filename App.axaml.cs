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
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        InitialiseMarkdown();

        License.iConfirmNonCommercialUse("Jack Ishmael");

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel() };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void InitialiseMarkdown()
    {
        MarkdownNode.Register<MathInlineNode>();
        MarkdownNode.Register<MathBlockNode>();
    }
}