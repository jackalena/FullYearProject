using System.Collections.Generic;
using System.Text.Json.Serialization;
using FullYearProject.Models.Quiz.Options;
using FullYearProject.Models.Quiz.Question.Parameters;

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
    ///     The number of times that this question can be re-asked in a game.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ReuseCount { get; set; } = 0;

    /// <summary>
    ///     The answer options for the question.
    /// </summary>
    public QuizQuestionOptionCollection Options { get; set; } = [];

    /// <summary>
    ///     The explanation for the correct answer to the question.
    /// </summary>
    public string? Explanation { get; set; }

    /// <summary>
    ///     The parameters/variables that will be used to calculate values used in the question.
    /// </summary>
    public List<QuizQuestionParameter>? Parameters { get; set; } = null;
}