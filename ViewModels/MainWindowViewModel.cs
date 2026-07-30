namespace FullYearProject.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        var introVm = new IntroViewModel();
        introVm.StartQuiz += StartGame;

        CurrentViewModel = introVm;
    }

    public ViewModelBase CurrentViewModel { get; set; }

    private void StartGame()
    {
    }
}