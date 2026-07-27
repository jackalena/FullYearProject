using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using FullYearProject.Configuration;
using FullYearProject.Helpers;
using FullYearProject.Models.Responses;

namespace FullYearProject.ViewModels;

public partial class ResultsViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CorrectPercentage))]
    [NotifyPropertyChangedFor(nameof(Grade))]
    [NotifyPropertyChangedFor(nameof(Message))]
    [NotifyPropertyChangedFor(nameof(TotalAnswered))]
    [NotifyPropertyChangedFor(nameof(CorrectAnswered))]
    public partial IEnumerable<QuestionResponseTopic> TopicResults { get; set; }

    public int? TotalAnswered => field ??= TopicResults?.Sum(topic => topic.Count);
    public int? CorrectAnswered => field ??= TopicResults?.Sum(topic => topic.Count(r => r.IsAnswerCorrect));

    public int CorrectPercentage => (int)(CorrectAnswered / (double)(TotalAnswered ?? 0) * 100 ?? 0);

    public GradeBoundary Grade => GradeBoundaries.GetGrade(CorrectPercentage);

    public string? Message => field ??= Grade?.Messages.Apply(msgs => Random.Shared.GetItem(msgs));
}