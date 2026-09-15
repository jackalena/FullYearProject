using System.Collections.Generic;
using System.Text.Json.Serialization;

using FullYearProject.Logging;

namespace FullYearProject.Models.Quiz.Question.Parameters;

/// <summary>
///     Represents a constraint that can be applied to a parameter.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
[JsonDerivedType(typeof(RangeQuizQuestionParameterConstraint), "Range")]
[JsonDerivedType(typeof(IntegerQuizQuestionParameterConstraint), "Integer")]
[JsonDerivedType(typeof(EqualQuizQuestionParameterConstraint), "Equal")]
public abstract class QuizQuestionParameterConstraint : Loggable
{
    /// <summary>
    ///     The type of the constraint.
    /// </summary>
    [JsonIgnore]
    public string Type { get; init; } = "Unknown";

    /// <summary>
    ///     The variables that can be used in the constraint.
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, double> Variables { get; set; } = new();

    /// <summary>
    ///     Applies the constraint to the given value of a parameter.
    /// </summary>
    /// <param name="value">The current value of the parameter.</param>
    /// <returns>The new value of the parameter.</returns>
    public abstract double Apply(double value);
}