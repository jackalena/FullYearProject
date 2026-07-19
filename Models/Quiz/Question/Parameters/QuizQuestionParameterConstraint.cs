using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FullYearProject.Models.Quiz.Question.Parameters;

/// <summary>
///     Represents a constraint that can be applied to a parameter.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
[JsonDerivedType(typeof(RangeQuizQuestionParameterConstraint), "Range")]
[JsonDerivedType(typeof(IntegerQuizQuestionParameterConstraint), "Integer")]
[JsonDerivedType(typeof(EqualQuizQuestionParameterConstraint), "Equal")]
public abstract class QuizQuestionParameterConstraint
{
    public string Type { get; set; } = "Unknown";

    /// <summary>
    ///     Applies the constraint to the given value of a parameter.
    /// </summary>
    /// <param name="value">The current value of the parameter.</param>
    /// <param name="variables">The existing variables and their values.</param>
    /// <returns>The new value of the parameter.</returns>
    public abstract double Apply(double value, Dictionary<string, double> variables);
}