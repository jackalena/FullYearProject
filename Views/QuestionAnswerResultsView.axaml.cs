using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls;

using FullYearProject.Models.Responses;

namespace FullYearProject.Views;

/// <summary>
///     Control to show a list of answered questions and their results.
/// </summary>
public partial class QuestionAnswerResultsView : UserControl
{
    /// <summary>
    ///     Identifies the <see cref="ItemsSource" /> dependency property.
    /// </summary>
    public static readonly StyledProperty<IEnumerable<QuestionResponseTopic>> ItemsSourceProperty =
        AvaloniaProperty.Register<QuestionAnswerResultsView, IEnumerable<QuestionResponseTopic>>(nameof(ItemsSource));

    /// <summary>
    ///     The list of questions and their responses.
    /// </summary>
    public IEnumerable<QuestionResponseTopic> ItemsSource {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="QuestionAnswerResultsView" /> class.
    /// </summary>
    public QuestionAnswerResultsView()
    {
        InitializeComponent();
    }
}