using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

/// <summary>
///     JSON converter to convert between <see cref="OptionIsCorrectDefinition" /> instances and JSON.
/// </summary>
public class IsCorrectDefinitionConverter : JsonConverter<OptionIsCorrectDefinition>
{
    /// <summary>
    ///     <inheritdoc />
    ///     Returns a <see cref="OptionIsCorrectDefinition" /> instance depending on the definition type.
    /// </summary>
    /// <inheritdoc />
    public override OptionIsCorrectDefinition? Read(ref Utf8JsonReader reader, Type typeToConvert,
                                                    JsonSerializerOptions options)
    {
        return reader.TokenType switch {
            JsonTokenType.True or JsonTokenType.False => new BooleanOptionIsCorrectDefinition { Value = reader.GetBoolean() },
            JsonTokenType.String => new ExpressionOptionIsCorrectDefinition {
                Expression = reader.GetString() ??
                             throw new JsonException("Cannot read expression string from JSON file")
            },
            _ => throw new JsonException("Invalid IsCorrect definition type")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, OptionIsCorrectDefinition value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }
}