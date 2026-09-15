using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FullYearProject.Models.Display;
using FullYearProject.Models.Quiz;
using FullYearProject.Models.Quiz.Options;
using FullYearProject.Models.Quiz.Question;

using Microsoft.Extensions.Logging;

namespace FullYearProject.ViewModels;

/// <summary>
///     View model for a quiz question.
/// </summary>
public partial class QuestionViewModel : ViewModelBase
{
    // Cancellation token source to skip the delay after a question is completed before the next question is shown
    private CancellationTokenSource? _completedDelayCts;

    /// <summary>
    ///     The question to show.
    /// </summary>
    public QuizQuestion Question { get; init; }

    /// <summary>
    ///     The quiz this question belongs to.
    /// </summary>
    public Quiz Quiz { get; init; }

    /// <summary>
    ///     The name of the topic this question belongs to.
    /// </summary>
    public string TopicName => Quiz.Topics.FromId(Question.Topic).Name;

    /// <summary>
    ///     Whether to show the message that the question has been answered and whether the answer was correct.
    /// </summary>
    [ObservableProperty]
    public partial bool ShowCompleted { get; set; }

    /// <summary>
    ///     Whether the question was answered correctly.
    /// </summary>
    [ObservableProperty]
    public partial bool IsCorrect { get; set; }

    /// <summary>
    ///     Whether the question was answered incorrectly.
    /// </summary>
    [ObservableProperty]
    public partial bool IsIncorrect { get; set; }

    /// <summary>
    ///     Whether the question was skipped.
    /// </summary>
    [ObservableProperty]
    public partial bool IsSkipped { get; set; }

    /// <summary>
    ///     The option that was selected, or null if no option was selected.
    /// </summary>
    [ObservableProperty]
    public partial QuizQuestionOption? SelectedOption { get; set; }

    /// <summary>
    ///     The correct option for the current question.
    /// </summary>
    public QuizQuestionOption? CorrectOption => field ??= FindCorrectOption();

    /// <summary>
    ///     Initialises a new instance of the <see cref="QuestionViewModel" /> class, using a default question and quiz.
    /// </summary>
    public QuestionViewModel()
    {
        Question = new();
        Quiz = new();
    }

    /// <summary>
    ///     Initialises a new instance of the <see cref="QuestionViewModel" /> class, using the specified question and quiz.
    /// </summary>
    /// <param name="question">The question to show.</param>
    /// <param name="quiz">The quiz this question belongs to.</param>
    public QuestionViewModel(GeneratedQuizQuestion question, Quiz quiz)
    {
        Question = question;
        Quiz = quiz;
    }

    /// <summary>
    ///     Invoked when the user answers a question.
    /// </summary>
    public event Action<QuizQuestionOption?>? QuestionAnswered;

    /// <summary>
    ///     Invoked when the next question is ready to be shown.
    /// </summary>
    public event Action? QuestionCompleted;

    [RelayCommand]
    private async Task AnswerButtonPressed(QuizQuestionOption option)
    {
        await ShowCompletedQuestion(option);
    }

    [RelayCommand]
    private async Task SkipButtonPressed()
    {
        await ShowCompletedQuestion(null);
    }

    // Shows the message that the question has been answered and whether the answer was correct.
    private async Task ShowCompletedQuestion(QuizQuestionOption? option)
    {
        SelectedOption = option;
        IsCorrect = option == CorrectOption;
        IsSkipped = option == null;
        IsIncorrect = !(IsCorrect || IsSkipped);

        QuestionAnswered?.Invoke(option);

        ShowCompleted = true;

        try
        {
            _completedDelayCts = new();

            if (IsCorrect)
            {
                // Wait 3 seconds before showing the next question
                await Task.Delay(3000, _completedDelayCts.Token);
            }
            else
            {
                // Infinite delay, only cancels if the user skips the question
                await Task.Delay(-1, _completedDelayCts.Token);
            }
        }
        catch (TaskCanceledException) { }

        _completedDelayCts?.Dispose();
        _completedDelayCts = null;

        QuestionCompleted?.Invoke();
    }

    // Cancels the delay after a question is completed
    [RelayCommand]
    private void SkipCompleted()
    {
        _completedDelayCts?.Cancel();
    }

    // Finds the correct option for the current question
    private QuizQuestionOption? FindCorrectOption()
    {
        var option = Question.Options.FirstOrDefault(option => option.IsCorrect.EvaluateIsCorrect(null));

        if (option == null)
        {
            Logger.LogWarning("Question has no correct option.");
        }

        return option;
    }
}