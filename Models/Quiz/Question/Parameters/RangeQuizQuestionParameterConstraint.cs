using System;
using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Question.Parameters;

/// <summary>
///     Represents a constraint that sets the value of a parameter to a random number in a specified range.
/// </summary>
public class RangeQuizQuestionParameterConstraint : QuizQuestionParameterConstraint
{
    public RangeQuizQuestionParameterConstraint()
    {
        Type = "Range";
    }

    /// <summary>
    ///     The range of values to choose from. Must be a list of two numbers.
    /// </summary>
    public List<double> Value { get; set; } = [0, 1];

    public override double Apply(double value, Dictionary<string, double> variables)
    {
        if (Value.Count != 2)
        {
            throw new InvalidOperationException("Value must be a list of two numbers");
        }

        return Random.Shared.NextDouble() * (Value[1] - Value[0]) + Value[0];
    }
}