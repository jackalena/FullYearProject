namespace FullYearProject.Models.Quiz.Question;

/// <summary>
///     Represents a quiz question.
/// </summary>
public class QuizQuestion
{
    /// <summary>
    ///     The id of the question topic.
    /// </summary>
    public int Topic { get; set; } = -1;

    /// <summary>
    ///     The text to show for the question.
    /// </summary>
    public string Text { get; set; } = "Question";

    /// <summary>
    ///     The answer options for the question.
    /// </summary>
    public QuizQuestionOptionCollection Options { get; set; } = [];

    /// <summary>
    ///     The explanation for the correct answer to the question.
    /// </summary>
    public string? Explanation { get; set; }
}