using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using FullYearProject.Models.Quiz;
using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.ViewModels;

public partial class QuestionViewModel : ViewModelBase
{
    public QuestionViewModel()
    {
        Question = new();
        Quiz = new();
    }

    public QuestionViewModel(QuizQuestion question, Quiz quiz)
    {
        Question = question;
        Quiz = quiz;
    }

    public QuizQuestion Question { get; }
    public Quiz Quiz { get; }

    [ObservableProperty] public partial Timer? Timer { get; set; }

    public string QuestionText => Question.Text;

    public string QuestionTopic =>
        Quiz.Topics.FirstOrDefault(t => t?.Id == Question.Topic, null)?.Name ?? "No Topic";

    public List<QuizQuestionOption> QuestionOptions => Question.Options;

    public IImage? QuestionImage => null;

    [ObservableProperty] public partial TimeSpan TimeRemaining { get; set; }

    partial void OnTimerChanged(Timer? oldValue, Timer? newValue)
    {
        oldValue?.
    }
}