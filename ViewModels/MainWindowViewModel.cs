using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FullYearProject.Models;
using FullYearProject.Models.Quiz;
using FullYearProject.Models.Quiz.Question;
using FullYearProject.Models.Responses;

namespace FullYearProject.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public enum QuestionPriority
    {
        Unanswered,
        Skipped,
        Incorrect,
        Correct
    }

    private readonly PrioritisedList<QuestionPriority, QuizQuestion> _questions =
        PrioritisedList<QuestionPriority, QuizQuestion>.FromEnum<QuestionPriority>();

    private readonly TimeRemainingProvider _timer = new();

    private Quiz? _quiz;

    public MainWindowViewModel()
    {
        var introVm = new IntroViewModel();
        introVm.StartQuiz += StartGame;

        CurrentViewModel = introVm;

        _timer.TimeRemainingChanged += (_, e) => TimeRemaining = e.TimeRemaining;
        _timer.TimeRemainingElapsed += OnTimerElapsed;

        _ = LoadGame();
    }

    [ObservableProperty] public partial TimeSpan TimeRemaining { get; set; }

    [ObservableProperty] public partial ViewModelBase CurrentViewModel { get; set; }

    [ObservableProperty] public partial bool ShowTimer { get; set; }

    private async Task LoadGame()
    {
        _quiz = await Quiz.LoadFile("SampleQuestions/set-1.json");
        (CurrentViewModel as IntroViewModel)?.Quiz = _quiz;

        _questions.Clear();
        _questions.AddRange(_quiz.Questions);
    }

    private void StartGame()
    {
        if (_quiz == null)
        {
            return;
        }

        ShowTimer = true;

        _timer.StartTime = _quiz.Settings.TimeLimit;
        _timer.Start();

        ShowNextQuestion();
    }

    private void ShowNextQuestion()
    {
        var question = _questions.GetNext();

        var questionVm = new QuestionViewModel(question, _quiz!);
        questionVm.QuestionAnswered += option => OnQuestionAnswered(question, option);
        questionVm.QuestionCompleted += ShowNextQuestion;
        CurrentViewModel = questionVm;
    }

    private void OnTimerElapsed()
    {
        CurrentViewModel = new ResultsViewModel();
    }

    private void OnQuestionAnswered(QuizQuestion question, QuizQuestionOption? option)
    {
    }
}