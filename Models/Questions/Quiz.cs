using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FullYearProject.Models.Questions;

public class Quiz
{
    public QuizSettings Settings { get; set; } = new();
    public QuizTopicCollection Topics { get; set; } = [];
    public QuizQuestionCollection Questions { get; set; } = [];
}

public class QuizSettings
{
    public string Title { get; set; } = "Quiz";
    public string? Description { get; set; }
    public TimeSpan TimeLimit { get; set; } = TimeSpan.FromMinutes(5);
}

public class QuizTopic
{
    public int Id { get; set; } = -1;
    public string Name { get; set; } = "Topic";
    public string? Description { get; set; }
}

public class QuizTopicCollection : List<QuizTopic>
{
}

public class QuizQuestion
{
    public int Topic { get; set; } = -1;
    public string Text { get; set; } = "Question";
    public QuizQuestionOptionCollection Options { get; set; } = [];
    public string? Explanation { get; set; }
}

public class QuizQuestionCollection : List<QuizQuestion>
{
}

public class QuizQuestionOption
{
    public string Type { get; set; } = "Unknown";
    public string Value { get; set; } = "Option";
    public string Format { get; set; } = "{0}";

    [JsonConverter(typeof(IsCorrectDefinitionConverter))]
    public OptionIsCorrectDefinition IsCorrect { get; set; } = BooleanOptionIsCorrectDefinition.Default;
}

public class QuizQuestionOptionCollection : List<QuizQuestionOption>
{
}

public class QuizQuestionParameter
{
    public string Name { get; set; } = "Unknown";
    public List<QuizQuestionParameterConstraint> Constraints { get; set; } = [];
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
[JsonDerivedType(typeof(RangeQuizQuestionParameterConstraint), "Range")]
[JsonDerivedType(typeof(IntegerQuizQuestionParameterConstraint), "Integer")]
[JsonDerivedType(typeof(EqualQuizQuestionParameterConstraint), "Equal")]
public abstract class QuizQuestionParameterConstraint
{
    public string Type { get; set; } = "Unknown";

    public abstract decimal Apply(decimal value);
}

public class RangeQuizQuestionParameterConstraint : QuizQuestionParameterConstraint
{
    public RangeQuizQuestionParameterConstraint()
    {
        Type = "Range";
    }

    public List<decimal> Value { get; set; } = [0, 1];

    public override decimal Apply(decimal value)
    {
        if (Value.Count != 2) throw new InvalidOperationException("Value must be a list of two numbers");

        return (decimal)Random.Shared.NextDouble() * (Value[1] - Value[0]) + Value[0];
    }
}

public class IntegerQuizQuestionParameterConstraint : QuizQuestionParameterConstraint
{
    public IntegerQuizQuestionParameterConstraint()
    {
        Type = "Integer";
    }

    public override decimal Apply(decimal value)
    {
        return Math.Round(value);
    }
}

public class EqualQuizQuestionParameterConstraint : QuizQuestionParameterConstraint
{
    public EqualQuizQuestionParameterConstraint()
    {
        Type = "Equal";
    }

    public string? Value { get; set; }

    public override decimal Apply(decimal value)
    {
        throw new NotImplementedException();
    }
}

public abstract class OptionIsCorrectDefinition
{
    public abstract bool EvaluateIsCorrect(object? state);
}

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
            JsonTokenType.String => new ExpressionOptionIsCorrectDefiniton
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

public class BooleanOptionIsCorrectDefinition : OptionIsCorrectDefinition
{
    public static BooleanOptionIsCorrectDefinition Default { get; } = new() { Value = false };

    public bool Value { get; set; }

    public override bool EvaluateIsCorrect(object? state)
    {
        return Value;
    }
}

public class ExpressionOptionIsCorrectDefiniton : OptionIsCorrectDefinition
{
    public string Expression { get; set; } = string.Empty;

    public override bool EvaluateIsCorrect(object? state)
    {
        return false;
    }
}