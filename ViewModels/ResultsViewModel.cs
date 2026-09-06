using System;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using FullYearProject.Configuration;
using FullYearProject.Helpers;
using FullYearProject.Models.Quiz;
using FullYearProject.Models.Responses;

namespace FullYearProject.ViewModels;

public partial class ResultsViewModel : ViewModelBase
{
    private readonly Quiz _quiz;

    public ResultsViewModel(QuestionResponseTopicCollection topicResults, Quiz quiz)
    {
        _quiz = quiz;
        TopicResults = topicResults;
    }

    public ResultsViewModel(QuestionResponseCollection responses, Quiz quiz)
    {
        _quiz = quiz;
        TopicResults = new(responses.GroupBy(r => r.Question.Topic)
            .Select(group => new QuestionResponseTopic(group) { Topic = quiz.Topics.FromId(group.Key) }));
    }

    public QuestionResponseTopicCollection TopicResults { get; }

    public int? TotalAnswered => field ??= TopicResults?.Sum(topic => topic.Count);
    public int? CorrectAnswered => field ??= TopicResults?.Sum(topic => topic.Count(r => r.IsAnswerCorrect));

    public int CorrectPercentage => (int)(CorrectAnswered / (double)(TotalAnswered ?? 0) * 100 ?? 0);

    public GradeBoundary Grade => GradeBoundaries.GetGrade(CorrectPercentage);

    public string? Message => field ??= Random.Shared.GetItem(Grade.Messages);

    public event Action? QuitRequested;
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