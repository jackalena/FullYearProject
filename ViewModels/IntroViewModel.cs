using System;
using System.Collections.ObjectModel;
using System.Linq;

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
    public partial QuizSettings[] Quizzes { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    public partial string HeaderString { get; set; } = "Select quizzes...";

    /// <summary>
    ///     The currently selected quizzes.
    /// </summary>
    public ObservableCollection<QuizSettings> SelectedQuizzes { get; } = [];

    /// <summary>
    ///     Initialises a new instance of the <see cref="IntroViewModel" /> class.
    /// </summary>
    public IntroViewModel()
    {
        SelectedQuizzes.CollectionChanged += (_, _) =>
        {
            if (SelectedQuizzes.Count == 0)
            {
                HeaderString = "Select quizzes...";

                return;
            }

            HeaderString = string.Join(", ", SelectedQuizzes.Select(q => q.Title));
        };
    }

    /// <summary>
    ///     Event raised when the user presses the button to start the quiz.
    /// </summary>
    public event Action<QuizSettings[]>? StartQuiz;

    /// <summary>
    ///     Starts the quiz.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanStart))]
    private void Start()
    {
        if (SelectedQuizzes.Any())
        {
            StartQuiz?.Invoke(Quizzes);
        }
    }

    private bool CanStart()
    {
        return SelectedQuizzes.Any();
    }
}