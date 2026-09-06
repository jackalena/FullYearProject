using Avalonia.Controls;

namespace FullYearProject.Views;

public partial class QuestionView : UserControl
{
    private static readonly string[] AnswerButtonClasses = ["Red", "Yellow", "Green", "Blue"];
    private static readonly string[] AnswerButtonTrueFalseClasses = ["Green", "Red"];

    public QuestionView()
    {
        InitializeComponent();
    }

    private void AnswerOptionsItemsControl_OnPreparingContainer(object? sender, ContainerPreparedEventArgs e)
    {
        var isTrueFalse = (sender as ItemsControl)?.ItemCount == 2;

        var newClass = isTrueFalse
            ? AnswerButtonTrueFalseClasses[e.Index]
            : AnswerButtonClasses[e.Index % AnswerButtonClasses.Length];

        e.Container.Classes.Add(newClass);
    }
}