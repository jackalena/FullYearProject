using System.Text.Json.Serialization;
using FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

namespace FullYearProject.Models.Quiz.Question;

public class QuizQuestionOption
{
    public string Type { get; set; } = "Unknown";
    public string Value { get; set; } = "Option";
    public string Format { get; set; } = "{0}";

    [JsonConverter(typeof(IsCorrectDefinitionConverter))]
    public OptionIsCorrectDefinition IsCorrect { get; set; } = BooleanOptionIsCorrectDefinition.Default;
}