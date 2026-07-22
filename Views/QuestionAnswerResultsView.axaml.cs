using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using FullYearProject.Models.Responses;

namespace FullYearProject.Views;

public partial class QuestionAnswerResultsView : UserControl
{
    public static readonly StyledProperty<IEnumerable<QuestionResponseTopic>> ItemsSourceProperty =
        AvaloniaProperty.Register<QuestionAnswerResultsView, IEnumerable<QuestionResponseTopic>>(
            nameof(ItemsSource));

    public QuestionAnswerResultsView()
    {
        InitializeComponent();
    }

    public IEnumerable<QuestionResponseTopic> ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
}