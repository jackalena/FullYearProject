namespace FullYearProject.Models.Quiz.Options;

/// <summary>
///     Represents an option for a quiz question that displays the result of a math expression.
/// </summary>
public class ExpressionQuizQuestionOption : QuizQuestionOption
{
    /// <summary>
    ///     The format to use for displaying the option.
    /// </summary>
    public string? Format { get; init; }

    /// <summary>
    ///     Initialises a new instance of the <see cref="ExpressionQuizQuestionOption" /> class.
    /// </summary>
    public ExpressionQuizQuestionOption()
    {
        Type = "Expression";
    }
}