using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using FullYearProject.Models.Questions;

namespace FullYearProject.Views;

public partial class QuestionAnswerResultsView : UserControl
{
    public static readonly StyledProperty<IEnumerable<QuestionResultTopic>> ItemsSourceProperty =
        AvaloniaProperty.Register<QuestionAnswerResultsView, IEnumerable<QuestionResultTopic>>(
            nameof(ItemsSource));

    public QuestionAnswerResultsView()
    {
        InitializeComponent();
    }

    public IEnumerable<QuestionResultTopic> ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
}