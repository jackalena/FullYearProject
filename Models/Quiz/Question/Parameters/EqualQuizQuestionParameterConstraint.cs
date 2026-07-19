using System.Collections.Generic;
using FullYearProject.Models.Questions;

namespace FullYearProject.Models.Quiz.Question.Parameters;

/// <summary>
///     Represents a constraint that makes the value of a parameter equal to the result of an expression.
/// </summary>
public class EqualQuizQuestionParameterConstraint : QuizQuestionParameterConstraint
{
    private NumericalExpressionEvaluator? _expressionEvaluator;

    public EqualQuizQuestionParameterConstraint()
    {
        Type = "Equal";
    }

    /// <summary>
    /// The expression to evaluate.
    /// </summary>
    public string? Value { get; set; }

    public override double Apply(double value, Dictionary<string, double> variables)
    {
        _expressionEvaluator ??= new(Value);
        _expressionEvaluator.Variables = variables;
        return _expressionEvaluator.Evaluate();
    }
}