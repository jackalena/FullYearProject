using Avalonia.Controls;

namespace FullYearProject.Views;

/// <summary>
///     Control to show a question and its options to the user.
/// </summary>
public partial class QuestionView : UserControl
{
    // The class names to apply to the answer buttons, used for styling
    private static readonly string[] AnswerButtonClasses = ["Red", "Yellow", "Green", "Blue"];
    private static readonly string[] AnswerButtonTrueFalseClasses = ["Green", "Red"];

    /// <summary>
    ///     Initialises a new instance of the <see cref="QuestionView" /> class.
    /// </summary>
    public QuestionView()
    {
        InitializeComponent();
    }

    // Called when the control displaying the answer options is preparing the answer options
    private void AnswerOptionsItemsControl_OnPreparingContainer(object? sender, ContainerPreparedEventArgs e)
    {
        var isTrueFalse = (sender as ItemsControl)?.ItemCount == 2;

        // Add the appropriate class to the answer button
        var newClass = isTrueFalse
            ? AnswerButtonTrueFalseClasses[e.Index]
            : AnswerButtonClasses[e.Index % AnswerButtonClasses.Length];

        e.Container.Classes.Add(newClass);
    }
}