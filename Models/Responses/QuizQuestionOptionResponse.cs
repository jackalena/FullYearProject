using FullYearProject.Models.Quiz.Options;

namespace FullYearProject.Models.Responses;

/// <summary>
///     Represents a response to a single option for a quiz question.
/// </summary>
public class QuizQuestionOptionResponse : QuizQuestionOption
{
    /// <summary>
    ///     Whether the user selected the option.
    /// </summary>
    public bool IsSelected { get; set; }
}