using System.Collections.Generic;
using System.Text.Json.Serialization;

using FullYearProject.Models.Display;
using FullYearProject.Models.Quiz.Options;
using FullYearProject.Models.Quiz.Question.Parameters;

namespace FullYearProject.Models.Quiz.Question;

/// <summary>
///     Represents a quiz question.
/// </summary>
public class QuizQuestion : IReusablePrioritisedListItem
{
    /// <summary>
    ///     The id of the question topic.
    /// </summary>
    public int Topic { get; init; } = -1;

    /// <summary>
    ///     The text to show for the question.
    /// </summary>
    public string Text { get; init; } = "Question";

    /// <summary>
    ///     The answer options for the question.
    /// </summary>
    public QuizQuestionOptionCollection Options { get; init; } = [];

    /// <summary>
    ///     The explanation for the correct answer to the question.
    /// </summary>
    public string? Explanation { get; init; }

    /// <summary>
    ///     The parameters/variables that will be used to calculate values used in the question.
    /// </summary>
    public List<QuizQuestionParameter>? Parameters { get; init; } = null;

    /// <inheritdoc />
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ReuseCount { get; init; } = 0;

    /// <inheritdoc />
    [JsonIgnore]
    public int CurrentReuseCount { get; set; } = 0;
}