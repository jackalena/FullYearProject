using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FullYearProject.Models;
using FullYearProject.Models.Quiz;

namespace FullYearProject.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly TimeRemainingProvider _timer = new();
    private Quiz? _quiz;

    public MainWindowViewModel()
    {
        var introVm = new IntroViewModel();
        introVm.StartQuiz += StartGame;

        CurrentViewModel = introVm;

        _ = LoadGame();
    }

    [ObservableProperty] public partial ViewModelBase CurrentViewModel { get; set; }

    private async Task LoadGame()
    {
        _quiz = await Quiz.LoadFile("SampleQuestions/set-1.json");
        (CurrentViewModel as IntroViewModel)?.Quiz = _quiz;
    }

    private void StartGame()
    {
        if (_quiz == null)
        {
            return;
        }

        _timer.StartTime = _quiz.Settings.TimeLimit;
        _timer.Start();

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        var question = _quiz!.Questions[0];

        CurrentViewModel = new QuestionViewModel(question, _quiz)
        {
            Timer = _timer
        };
    }

    private void OnTimerElapsed()
    {
        CurrentViewModel = new ResultsViewModel();
    }
}