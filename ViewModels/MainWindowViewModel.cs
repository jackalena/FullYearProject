using System;
using System.Collections.Generic;
using System.IO;
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

/// <summary>
///     The view model for the application's main window.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private const string QuizDirectory = "Questions";

    private readonly PrioritisedList<QuestionPriority, QuizQuestion> _questions =
        PrioritisedList<QuestionPriority, QuizQuestion>.FromEnum<QuestionPriority>();

    private readonly QuestionResponseCollection _responses = [];

    private readonly TimeRemainingProvider _timer = new();

    private volatile bool _hasTimerElapsed;

    private Quiz? _quiz;

    /// <summary>
    ///     The time remaining until the quiz ends.
    /// </summary>
    [ObservableProperty]
    public partial TimeSpan TimeRemaining { get; set; }

    /// <summary>
    ///     The current view model to display.
    /// </summary>
    [ObservableProperty]
    public partial ViewModelBase CurrentViewModel { get; set; }

    /// <summary>
    ///     Whether the timer should be shown.
    /// </summary>
    [ObservableProperty]
    public partial bool ShowTimer { get; set; }

    /// <summary>
    ///     The title of the quiz.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(WindowTitle))]
    public partial string QuizTitle { get; set; } = string.Empty;

    /// <summary>
    ///     The title to show in the window.
    /// </summary>
    public string WindowTitle => $"Quiz - {QuizTitle}";

    /// <summary>
    ///     Creates a new instance of the <see cref="MainWindowViewModel" /> class.
    /// </summary>
    public MainWindowViewModel()
    {
        CurrentViewModel = null!;
        ShowIntro();

        _timer.TimeRemainingChanged += (_, e) => TimeRemaining = e.TimeRemaining;
        _timer.TimeRemainingElapsed += OnTimerElapsed;
    }

    // Loads the QuizSettings data from each quiz in the questions directory.
    private async Task<QuizSettings[]> LoadQuizNames()
    {
        List<QuizSettings> settings = new();

        // Loop through each json file in the questions directory and load the quiz settings.
        foreach (var file in Directory.EnumerateFiles(QuizDirectory, "*.json"))
        {
            try
            {
                settings.Add(await Quiz.LoadSettingsAsync(file));
            }
            catch (Exception e)
            {
                Logger.LogError("Could not load quiz: {Exception}", e.Message);
            }
        }

        return settings.ToArray();
    }

    // Loads the quiz data from a file.
    private async Task LoadGame(string filename)
    {
        try
        {
            _quiz = await Quiz.LoadFileAsync(filename);
            QuizTitle = _quiz.Settings.Title;
        }
        catch (Exception e)
        {
            Logger.LogError("Could not load game: {Exception}", e.Message);

            return;
        }

        _questions.Clear();
        _questions.AddRange(_quiz.Questions);
    }

    // Shows the intro screen / view model.
    private void ShowIntro()
    {
        var introVm = new IntroViewModel();
        introVm.StartQuiz += settings =>
        {
            if (settings.FileName != null)
            {
                InitQuiz(settings.FileName).ConfigureAwait(false);
            }
        };

        CurrentViewModel = introVm;

        LoadQuizNames()
           .ContinueWith(t => introVm.Quizzes = t.Result.ToList())
           .ConfigureAwait(false);
    }

    // Loads the quiz data from a file and starts the quiz.
    private async Task InitQuiz(string filename)
    {
        await LoadGame(filename).ConfigureAwait(false);
        StartGame();
    }

    // Starts the quiz.
    private void StartGame()
    {
        if (_quiz == null)
        {
            Logger.LogError("Attempted to start game without a quiz.");

            return;
        }

        ShowTimer = true;

        // (Re)start the timer.
        if (_timer.IsRunning)
        {
            _timer.Stop();
        }

        _hasTimerElapsed = false;
        _timer.StartTime = _quiz.Settings.TimeLimit;
        _timer.Start();

        _responses.Clear();
        ShowNextQuestion();
    }

    // Generates and shows the next quiz question.
    private void ShowNextQuestion()
    {
        if (_hasTimerElapsed)
        {
            return;
        }

        var question = GeneratedQuizQuestion.GenerateQuestion(_questions.GetNext());

        // Create a view model for the question.
        var questionVm = new QuestionViewModel(question, _quiz!);
        questionVm.QuestionAnswered += option => OnQuestionAnswered(question, option);
        questionVm.QuestionCompleted += ShowNextQuestion;

        CurrentViewModel = questionVm;
    }

    private void OnTimerElapsed()
    {
        _hasTimerElapsed = true;

        ShowTimer = false;

        // Show the results screen view model.
        var vm = new ResultsViewModel(_responses, _quiz!);
        vm.QuitRequested += () => Environment.Exit(0);
        vm.RestartRequested += RestartGame;

        CurrentViewModel = vm;
    }

    private void RestartGame()
    {
        ShowIntro();
    }

    private void OnQuestionAnswered(GeneratedQuizQuestion question, QuizQuestionOption? option)
    {
        // Create a QuestionResponse for the question and add it to the list of responses.
        var optionResponses = question.Options.Select(o => new QuestionOptionResponse(o, o == option));
        var response = new QuestionResponse(question, optionResponses);

        _responses.Add(response);
    }
}