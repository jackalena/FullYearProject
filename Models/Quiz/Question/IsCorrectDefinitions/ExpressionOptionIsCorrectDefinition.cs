using FullYearProject.Models.Quiz.Expressions;
using Microsoft.Extensions.Logging;

namespace FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

/// <summary>
///     Represents a definition of how to determine whether an option is correct using an expression.
/// </summary>
public class ExpressionOptionIsCorrectDefinition : OptionIsCorrectDefinition
{
    private BooleanExpressionEvaluator? _expressionEvaluator;

    /// <summary>
    ///     The expression to use to evaluate whether the option is correct.
    /// </summary>
    public string Expression { get; init; } = string.Empty;

    /// <summary>
    ///     <inheritdoc />
    ///     Will return the result of evaluating <see cref="Expression" /> for
    ///     <see cref="ExpressionOptionIsCorrectDefinition" />.
    /// </summary>
    /// <inheritdoc />
    public override bool EvaluateIsCorrect()
    {
        if (string.IsNullOrWhiteSpace(Expression))
        {
            Logger.LogWarning("Tried to evaluate empty expression.");

            return false;
        }

        _expressionEvaluator ??= new(Expression);
        _expressionEvaluator.Variables = Variables;

        return _expressionEvaluator.Evaluate();
    }
}