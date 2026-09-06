using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FullYearProject.Models;
using FullYearProject.Models.Display;
using FullYearProject.Models.Quiz;
using FullYearProject.Models.Quiz.Options;
using FullYearProject.Models.Quiz.Question;
using FullYearProject.Models.Responses;
using Microsoft.Extensions.Logging;

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

    //TODO: Reusable questions not being reused
    // tooltip colour
    // answer result formatting

    private readonly PrioritisedList<QuestionPriority, QuizQuestion> _questions =
        PrioritisedList<QuestionPriority, QuizQuestion>.FromEnum<QuestionPriority>();

    private readonly QuestionResponseCollection _responses = [];

    private readonly TimeRemainingProvider _timer = new();

    private volatile bool _hasTimerElapsed;

    private Quiz? _quiz;

    public MainWindowViewModel()
    {
        CurrentViewModel = null!;
        ShowIntro();

        _timer.TimeRemainingChanged += (_, e) => TimeRemaining = e.TimeRemaining;
        _timer.TimeRemainingElapsed += OnTimerElapsed;

        _ = LoadGame();
    }

    [ObservableProperty] public partial TimeSpan TimeRemaining { get; set; }

    [ObservableProperty] public partial ViewModelBase CurrentViewModel { get; set; }

    [ObservableProperty] public partial bool ShowTimer { get; set; }


    private async Task LoadGame()
    {
        try
        {
            _quiz = await Quiz.LoadFile("SampleQuestions/set-1.json");
        }
        catch (Exception e)
        {
            Logger.LogError("Could not load game: {Exception}", e.Message);

            return;
        }

        (CurrentViewModel as IntroViewModel)?.Quiz = _quiz;

        _questions.Clear();
        _questions.AddRange(_quiz.Questions);
    }

    private void ShowIntro()
    {
        var introVm = new IntroViewModel();
        introVm.StartQuiz += StartGame;

        CurrentViewModel = introVm;
    }

    private void StartGame()
    {
        if (_quiz == null)
        {
            return;
        }

        ShowTimer = true;

        _responses.Clear();
        _hasTimerElapsed = false;
        _timer.StartTime = _quiz.Settings.TimeLimit;
        _timer.Start();

        ShowNextQuestion();
    }

    private void ShowNextQuestion()
    {
        if (_hasTimerElapsed)
        {
            return;
        }

        var question = _questions.GetNext();

        var generatedQuestion = GeneratedQuizQuestion.GenerateQuestion(question);


        var questionVm = new QuestionViewModel(generatedQuestion, _quiz!);
        questionVm.QuestionAnswered += option => OnQuestionAnswered(question, option);
        questionVm.QuestionCompleted += ShowNextQuestion;
        CurrentViewModel = questionVm;
    }

    private void OnTimerElapsed()
    {
        _hasTimerElapsed = true;

        var vm = new ResultsViewModel(_responses, _quiz!);

        vm.QuitRequested += () => Environment.Exit(0);
        vm.RestartRequested += RestartGame;

        CurrentViewModel = vm;
    }

    private void RestartGame()
    {
        StartGame();
    }

    private void OnQuestionAnswered(QuizQuestion question, QuizQuestionOption? option)
    {
        var optionResponses = question.Options.Select(o => new QuestionOptionResponse(o, o == option));
        var response = new QuestionResponse(question, optionResponses);

        _responses.Add(response);
    }
}