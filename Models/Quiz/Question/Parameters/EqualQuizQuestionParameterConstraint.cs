using System.Collections.Generic;
using FullYearProject.Models.Questions;

namespace FullYearProject.Models.Quiz.Question.Parameters;

public class EqualQuizQuestionParameterConstraint : QuizQuestionParameterConstraint
{
    private NumericalExpressionEvaluator? _expressionEvaluator;

    public EqualQuizQuestionParameterConstraint()
    {
        Type = "Equal";
    }

    public string? Value { get; set; }

    public override double Apply(double value, Dictionary<string, double> variables)
    {
        _expressionEvaluator ??= new(Value);
        _expressionEvaluator.Variables = variables;
        return _expressionEvaluator.Evaluate();
    }
}