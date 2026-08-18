using System;
using System.Diagnostics;
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

    private readonly PrioritisedList<QuestionPriority, QuizQuestion> _questions =
        PrioritisedList<QuestionPriority, QuizQuestion>.FromEnum<QuestionPriority>();

    private readonly QuestionResponseCollection _responses = [];

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
        QuizQuestion question = null!;

        for (var i = 0; i < 10; i++)
        {
            question = _questions.GetNext();

            if (question.Parameters?.Count > 0)
            {
                break;
            }
        }

        var generatedQuestion = GeneratedQuizQuestion.GenerateQuestion(question);

        Debug.WriteLine(generatedQuestion.Text);
        foreach (var option in generatedQuestion.Options)
        {
            Debug.WriteLine(option.Value);
        }

        var questionVm = new QuestionViewModel(generatedQuestion, _quiz!);
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
        var optionResponses = question.Options.Select(o => new QuestionOptionResponse(o, o == option));
        var response = new QuestionResponse(question, optionResponses);

        _responses.Add(response);
    }
}