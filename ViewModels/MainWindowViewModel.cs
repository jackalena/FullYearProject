using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FullYearProject.Models;
using FullYearProject.Models.Quiz;
using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly TimeRemainingProvider _timer = new();
    private List<QuizQuestion> _correctQuestions = [];
    private List<QuizQuestion> _incorrectQuestions = [];

    private Quiz? _quiz;
    private List<QuizQuestion> _skippedQuestions = [];

    private List<QuizQuestion> _unusedQuestions = [];

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

        CurrentViewModel = new QuestionViewModel(question, _quiz, _timer);
    }

    private void OnTimerElapsed()
    {
        CurrentViewModel = new ResultsViewModel();
    }
}