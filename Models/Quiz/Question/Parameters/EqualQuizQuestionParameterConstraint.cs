using FullYearProject.Models.Quiz.Expressions;
using Microsoft.Extensions.Logging;

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
    ///     The expression to evaluate.
    /// </summary>
    public string Value { get; init; } = string.Empty;

    /// <inheritdoc />
    public override double Apply(double value)
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            Logger.LogWarning("Tried to evaluate empty expression.");

            return 0;
        }

        _expressionEvaluator ??= new(Value);
        _expressionEvaluator.Variables = Variables;
        return _expressionEvaluator.Evaluate();
    }
}