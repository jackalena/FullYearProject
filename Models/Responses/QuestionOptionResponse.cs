using FullYearProject.Models.Quiz.Options;

namespace FullYearProject.Models.Responses;

/// <summary>
///     Represents a response to a single option for a quiz question.
/// </summary>
public class QuestionOptionResponse : QuizQuestionOption
{
    public QuestionOptionResponse(QuizQuestionOption option, bool isSelected)
    {
        IsSelected = isSelected;
        Type = option.Type;
        Value = option.Value;
        IsCorrect = option.IsCorrect;
    }

    /// <summary>
    ///     Whether the option is correct or not.
    /// </summary>
    public bool IsCorrectBool => IsCorrect.EvaluateIsCorrect(null);

    /// <summary>
    ///     Whether the user selected the option.
    /// </summary>
    public bool IsSelected { get; set; }
}