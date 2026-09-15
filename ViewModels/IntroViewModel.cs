using System;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FullYearProject.Models.Quiz;

namespace FullYearProject.ViewModels;

/// <summary>
///     The view model for the application's introduction screen.
/// </summary>
public partial class IntroViewModel : ViewModelBase
{
    /// <summary>
    ///     The quizzes to choose from.
    /// </summary>
    [ObservableProperty]
    public partial List<QuizSettings> Quizzes { get; set; } = [];

    /// <summary>
    ///     The currently selected quiz.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    public partial QuizSettings? SelectedQuiz { get; set; }

    /// <summary>
    ///     Event raised when the user presses the button to start the quiz.
    /// </summary>
    public event Action<QuizSettings>? StartQuiz;

    /// <summary>
    ///     Starts the quiz.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanStart))]
    private void Start()
    {
        if (SelectedQuiz != null)
        {
            StartQuiz?.Invoke(SelectedQuiz);
        }
    }

    private bool CanStart()
    {
        return SelectedQuiz != null;
    }
}