using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;

namespace FullYearProject.Models.Quiz.Expressions;

/// <summary>
///     Evaluates a mathematical expression using a set of variables and their values.
/// </summary>
public abstract class ExpressionEvaluator
{
    // Expression to evaluate
    private readonly string _expressionStr;
    private Expression? _expression;

    static ExpressionEvaluator()
    {
        // Allows question variables names to override built-in constants like 'e'
        mXparser.setToOverrideBuiltinTokens();
    }

    /// <summary>
    ///     Creates a new ExpressionEvaluator using the expression provided.
    /// </summary>
    /// <param name="expression">The expression to use in evaluation.</param>
    public ExpressionEvaluator(string expression)
    {
        _expressionStr = expression;
    }

    /// <summary>
    ///     The variables and their values to use in the expression.
    /// </summary>
    public Dictionary<string, double> Variables { get; set; } = new();

    /// <summary>
    ///     Evaluates the expression using the variables provided.*
    ///     A boolean expression returns 0 or 1 for true and false.
    /// </summary>
    /// <returns>The value of the expression evaluated by mXparser</returns>
    protected double EvaluateInternal()
    {
        _expression ??= new(_expressionStr);
        _expression.removeAllConstants();

        foreach (var variable in Variables)
        {
            _expression.defineConstant(variable.Key, variable.Value);
        }

        var result = _expression.calculate();

        return result;
    }
}