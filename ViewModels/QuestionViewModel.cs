using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FullYearProject.Models;
using FullYearProject.Models.Quiz;
using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.ViewModels;

public partial class QuestionViewModel : ViewModelBase
{
    public Action<QuizQuestionOption?>? OnQuestionCompleted;

    public QuestionViewModel()
    {
        Question = new();
        Quiz = new();
    }

    public QuestionViewModel(QuizQuestion question, Quiz quiz, TimeRemainingProvider timer)
    {
        Question = question;
        Quiz = quiz;

        Timer = timer;
        Timer.TimeRemainingChanged += (_, e) => { TimeRemaining = e.TimeRemaining; };
        TimeRemaining = Timer.TimeRemaining;
    }

    public QuizQuestion Question { get; init; }
    public Quiz Quiz { get; init; }

    public TimeRemainingProvider? Timer { get; init; }

    public string QuestionText => Question.Text;

    public string QuestionTopic =>
        Quiz.Topics.FirstOrDefault(t => t?.Id == Question.Topic, null)?.Name ?? "No Topic";

    public List<QuizQuestionOption> QuestionOptions => Question.Options;

    public IImage? QuestionImage => null;

    [ObservableProperty] public partial TimeSpan TimeRemaining { get; set; }

    [ObservableProperty] public partial bool ShowCompleted { get; set; }
    [ObservableProperty] public partial bool IsCorrect { get; set; }

    [ObservableProperty] public partial QuizQuestionOption? SelectedOption { get; set; }
    public QuizQuestionOption CorrectOption => field ??= Question.Options.FindCorrectOption();

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

    private async Task ShowCompletedQuestion(QuizQuestionOption? option)
    {
        SelectedOption = option;
        IsCorrect = option == CorrectOption;

        ShowCompleted = true;

        if (IsCorrect)
        {
            await Task.Delay(500);
        }
        else
        {
            await Task.Delay(1500);
        }
    }
}