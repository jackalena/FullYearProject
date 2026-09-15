using FullYearProject.Models.Quiz.Options;

namespace FullYearProject.Models.Responses;

/// <summary>
///     Represents a response to a single option for a quiz question.
/// </summary>
public class QuestionOptionResponse : QuizQuestionOption
{
    /// <summary>
    ///     Whether the option is correct or not.
    /// </summary>
    public bool IsCorrectBool => IsCorrect.EvaluateIsCorrect(null);

    /// <summary>
    ///     Whether the user selected the option.
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    ///     Initialises a new instance of the <see cref="QuestionOptionResponse" /> class from a question option.
    /// </summary>
    /// <param name="option">The question option.</param>
    /// <param name="isSelected">Whether this option was selected.</param>
    public QuestionOptionResponse(QuizQuestionOption option, bool isSelected)
    {
        IsSelected = isSelected;
        Type = option.Type;
        Value = option.Value;
        IsCorrect = option.IsCorrect;
    }
}