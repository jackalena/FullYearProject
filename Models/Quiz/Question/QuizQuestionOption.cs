using System.Text.Json.Serialization;
using FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

namespace FullYearProject.Models.Quiz.Question;

public class QuizQuestionOption
{
    /// <summary>
    ///     The type of the option.
    /// </summary>
    public string Type { get; set; } = "Unknown";

    /// <summary>
    ///     The value of the option.
    /// </summary>
    public string Value { get; set; } = "Option";

    /// <summary>
    ///     The format to use for displaying the option.
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    ///     The definition of whether the option is correct.
    /// </summary>
    [JsonConverter(typeof(IsCorrectDefinitionConverter))]
    public OptionIsCorrectDefinition IsCorrect { get; set; } = BooleanOptionIsCorrectDefinition.Default;
}