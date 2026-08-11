using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FullYearProject.Models.Quiz;
using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.ViewModels;

public partial class QuestionViewModel : ViewModelBase
{
    public Action<QuizQuestionOption?>? QuestionAnswered;
    public Action? QuestionCompleted;
    private CancellationTokenSource? _completedDelayCts;

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

    public QuizQuestion Question { get; init; }
    public Quiz Quiz { get; init; }

    public string QuestionText => Question.Text;

    public string QuestionTopic =>
        Quiz.Topics.FirstOrDefault(t => t?.Id == Question.Topic, null)?.Name ?? "No Topic";

    public List<QuizQuestionOption> QuestionOptions => Question.Options;

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

        try
        {
            _completedDelayCts = new();

            if (IsCorrect)
            {
                await Task.Delay(3000, _completedDelayCts.Token);
            }
            else
            {
                // Infinite delay, only cancels if the user skips the question
                await Task.Delay(-1, _completedDelayCts.Token);
            }
        }
        catch (TaskCanceledException)
        {
        }

        _completedDelayCts?.Dispose();
        _completedDelayCts = null;

        QuestionCompleted?.Invoke();
    }

    [RelayCommand]
    private void SkipCompleted()
    {
        _completedDelayCts?.Cancel();
    }
}