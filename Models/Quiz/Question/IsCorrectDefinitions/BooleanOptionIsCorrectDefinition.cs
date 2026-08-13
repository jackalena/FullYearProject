using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

/// <summary>
///     Represents a definition of how to determine whether an option is correct using a fixed boolean value.
/// </summary>
public class BooleanOptionIsCorrectDefinition : OptionIsCorrectDefinition
{
    /// <summary>
    ///     Default value for <see cref="BooleanOptionIsCorrectDefinition" />, containing the value false.
    /// </summary>
    public static BooleanOptionIsCorrectDefinition Default { get; } = new() { Value = false };

    /// <summary>
    ///     Whether the option is correct or not.
    /// </summary>
    public bool Value { get; set; }

    /// <summary>
    ///     <inheritdoc />
    ///     Will always return <see cref="Value" /> for <see cref="BooleanOptionIsCorrectDefinition" />.
    /// </summary>
    /// <inheritdoc />
    public override bool EvaluateIsCorrect(Dictionary<string, double>? variables)
    {
        return Value;
    }
}