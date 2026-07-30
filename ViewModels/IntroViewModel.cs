using System;
using CommunityToolkit.Mvvm.Input;

namespace FullYearProject.ViewModels;

public partial class IntroViewModel : ViewModelBase
{
    public event Action? StartQuiz;

    [RelayCommand]
    private void Start()
    {
        StartQuiz?.Invoke();
    }
}