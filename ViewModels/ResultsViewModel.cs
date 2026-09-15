using System;
using System.Linq;

using CommunityToolkit.Mvvm.Input;

using FullYearProject.Configuration;
using FullYearProject.Helpers;
using FullYearProject.Models.Quiz;
using FullYearProject.Models.Responses;

namespace FullYearProject.ViewModels;

/// <summary>
///     View model for the results screen.
/// </summary>
public partial class ResultsViewModel : ViewModelBase
{
    private readonly Quiz _quiz;

    /// <summary>
    ///     The collection of question responses, grouped by topic.
    /// </summary>
    public QuestionResponseTopicCollection TopicResults { get; }

    /// <summary>
    ///     The total number of questions that have been answered.
    /// </summary>
    public int? TotalAnswered => field ??= TopicResults?.Sum(topic => topic.Count);

    /// <summary>
    ///     The number of questions that have been answered correctly.
    /// </summary>
    public int? CorrectAnswered => field ??= TopicResults?.Sum(topic => topic.Count(r => r.IsAnswerCorrect));

    /// <summary>
    ///     The percentage of questions that have been answered correctly.
    /// </summary>
    public int CorrectPercentage => (int) (CorrectAnswered / (double) (TotalAnswered ?? 0) * 100 ?? 0);

    /// <summary>
    ///     The grade for the quiz.
    /// </summary>
    public GradeBoundary Grade => GradeBoundaries.GetGrade(CorrectPercentage);

    /// <summary>
    ///     The message to display to the user for their grade.
    /// </summary>
    public string? Message => field ??= Random.Shared.GetItem(Grade.Messages);

    /// <summary>
    ///     Initialises a new instance of the <see cref="ResultsViewModel" /> class from a collection of question responses by
    ///     topic and a quiz.
    /// </summary>
    /// <param name="topicResults">A collection of question responses, grouped by topic.</param>
    /// <param name="quiz">The quiz the questions belong to.</param>
    public ResultsViewModel(QuestionResponseTopicCollection topicResults, Quiz quiz)
    {
        _quiz = quiz;
        TopicResults = topicResults;
    }

    /// <summary>
    ///     Initialises a new instance of the <see cref="ResultsViewModel" /> class from a collection of question responses and
    ///     a quiz.
    /// </summary>
    /// <param name="responses">A collection of question responses, not grouped by topic.</param>
    /// <param name="quiz">The quiz the questions belong to.</param>
    public ResultsViewModel(QuestionResponseCollection responses, Quiz quiz)
    {
        _quiz = quiz;

        // Group the responses by topic.
        TopicResults = new(responses.GroupBy(r => r.Question.Topic)
                                    .Select(group => new QuestionResponseTopic(group) { Topic = quiz.Topics.FromId(group.Key) }));
    }

    /// <summary>
    ///     Invoked when the user requests to quit the quiz.
    /// </summary>
    public event Action? QuitRequested;

    /// <summary>
    ///     Invoked when the user requests to restart the quiz.
    /// </summary>
    public event Action? RestartRequested;

    [RelayCommand]
    private void Quit()
    {
        QuitRequested?.Invoke();
    }

    [RelayCommand]
    private void Restart()
    {
        RestartRequested?.Invoke();
    }
}