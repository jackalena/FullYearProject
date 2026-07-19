using System;
using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Question.Parameters;

/// <summary>
///     Represents a constraint that rounds the value of a parameter to the nearest integer.
/// </summary>
public class IntegerQuizQuestionParameterConstraint : QuizQuestionParameterConstraint
{
    public IntegerQuizQuestionParameterConstraint()
    {
        Type = "Integer";
    }

    public override double Apply(double value, Dictionary<string, double> variables)
    {
        return Math.Round(value);
    }
}