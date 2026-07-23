using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace FullYearProject.Views;

public partial class QuestionView : UserControl
{
    public static readonly StyledProperty<string> QuestionProperty =
        AvaloniaProperty.Register<QuestionView, string>(nameof(Question));

    public static readonly StyledProperty<IImage?> ImageProperty =
        AvaloniaProperty.Register<QuestionView, IImage?>(nameof(Image));

    public static readonly StyledProperty<object?> ChildProperty =
        AvaloniaProperty.Register<QuestionView, object?>(
            nameof(Child));

    public static readonly StyledProperty<IEnumerable<string>> AnswerOptionsProperty =
        AvaloniaProperty.Register<QuestionView, IEnumerable<string>>(nameof(AnswerOptions));

    public static readonly StyledProperty<string> TopicNameProperty = AvaloniaProperty.Register<QuestionView, string>(
        nameof(TopicName));

    public static readonly StyledProperty<TimeSpan> TimeRemainingProperty =
        AvaloniaProperty.Register<QuestionView, TimeSpan>(
            nameof(TimeRemaining));

    private static readonly string[] AnswerButtonClasses = ["Red", "Yellow", "Green", "Blue"];
    private static readonly string[] AnswerButtonTrueFalseClasses = ["Red", "Green"];

    public QuestionView()
    {
        InitializeComponent();
    }

    public TimeSpan TimeRemaining
    {
        get => GetValue(TimeRemainingProperty);
        set => SetValue(TimeRemainingProperty, value);
    }

    public string TopicName
    {
        get => GetValue(TopicNameProperty);
        set => SetValue(TopicNameProperty, value);
    }

    public object? Child
    {
        get => GetValue(ChildProperty);
        set => SetValue(ChildProperty, value);
    }

    public IImage? Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public string Question
    {
        get => GetValue(QuestionProperty);
        set => SetValue(QuestionProperty, value);
    }

    public IEnumerable<string> AnswerOptions
    {
        get => GetValue(AnswerOptionsProperty);
        set => SetValue(AnswerOptionsProperty, value);
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