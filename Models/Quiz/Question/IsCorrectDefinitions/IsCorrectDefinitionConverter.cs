using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

public class IsCorrectDefinitionConverter : JsonConverter<OptionIsCorrectDefinition>
{
    public override OptionIsCorrectDefinition? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True or JsonTokenType.False => new BooleanOptionIsCorrectDefinition
            {
                Value = reader.GetBoolean()
            },
            JsonTokenType.String => new ExpressionOptionIsCorrectDefinition
            {
                Expression = reader.GetString() ??
                             throw new JsonException("Cannot read expression string from JSON file")
            },
            _ => throw new JsonException("Invalid IsCorrect definition type")
        };
    }

    public override void Write(Utf8JsonWriter writer, OptionIsCorrectDefinition value, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }
}