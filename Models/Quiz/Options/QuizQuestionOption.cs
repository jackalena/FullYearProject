using System.Text.Json.Serialization;
using FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

namespace FullYearProject.Models.Quiz.Options;

/// <summary>
///     Represents an option for a quiz question.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
[JsonDerivedType(typeof(TextQuizQuestionOption), "Text")]
[JsonDerivedType(typeof(ExpressionQuizQuestionOption), "Expression")]
public abstract class QuizQuestionOption
{
    /// <summary>
    ///     The type of the option.
    /// </summary>
    [JsonIgnore]
    public string Type { get; set; } = "Unknown";

    /// <summary>
    ///     The value of the option.
    /// </summary>
    public string Value { get; set; } = "Option";

    /// <summary>
    ///     The definition of whether the option is correct.
    /// </summary>
    [JsonConverter(typeof(IsCorrectDefinitionConverter))]
    public OptionIsCorrectDefinition IsCorrect { get; set; } = BooleanOptionIsCorrectDefinition.Default;
}