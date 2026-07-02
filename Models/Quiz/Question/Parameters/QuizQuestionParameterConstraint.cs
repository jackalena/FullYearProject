using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FullYearProject.Models.Quiz.Question.Parameters;

[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
[JsonDerivedType(typeof(RangeQuizQuestionParameterConstraint), "Range")]
[JsonDerivedType(typeof(IntegerQuizQuestionParameterConstraint), "Integer")]
[JsonDerivedType(typeof(EqualQuizQuestionParameterConstraint), "Equal")]
public abstract class QuizQuestionParameterConstraint
{
    public string Type { get; set; } = "Unknown";

    public abstract double Apply(double value, Dictionary<string, double> variables);
}