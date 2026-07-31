using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FullYearProject.Models.Quiz;

namespace FullYearProject.ViewModels;

public partial class IntroViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    public partial Quiz? Quiz { get; set; } = null;

    public event Action? StartQuiz;

    [RelayCommand(CanExecute = nameof(CanStart))]
    private void Start()
    {
        StartQuiz?.Invoke();
    }

    private bool CanStart()
    {
        return Quiz != null;
    }
}