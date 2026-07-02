using System;
using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Question.Parameters;

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