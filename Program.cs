using System;
using System.Threading.Tasks;

using Avalonia;

using FullYearProject.Models;
using FullYearProject.Views;

namespace FullYearProject;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Contains("--export-questions"))
        {
            MainWindow.Initialised += topLevel =>
            {
                topLevel.Hide();
                _ = ExportQuestion();

                return;

                async Task ExportQuestion()
                {
                    QuestionExporter exporter = new();
                    await exporter.ExportQuestion(topLevel);
                    Environment.Exit(0);
                }
            };
        }

        BuildAvaloniaApp()
           .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
                                                             .UsePlatformDetect()
                                                          #if DEBUG
                                                             .WithDeveloperTools()
                                                          #endif
                                                             .WithInterFont()
                                                             .LogToTrace();
}