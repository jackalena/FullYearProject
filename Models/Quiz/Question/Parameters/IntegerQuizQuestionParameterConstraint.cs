using System;

namespace FullYearProject.Models.Quiz.Question.Parameters;

/// <summary>
///     Represents a constraint that rounds the value of a parameter to the nearest integer.
/// </summary>
public class IntegerQuizQuestionParameterConstraint : QuizQuestionParameterConstraint
{
    /// <summary>
    ///     Initialises a new instance of the <see cref="IntegerQuizQuestionParameterConstraint" /> class.
    /// </summary>
    public IntegerQuizQuestionParameterConstraint()
    {
        Type = "Integer";
    }

    /// <inheritdoc />
    public override double Apply(double value)
    {
        return Math.Round(value);
    }
}