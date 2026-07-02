using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;

namespace FullYearProject.Models.Questions;

public abstract class ExpressionEvaluator
{
    private readonly string _expressionStr;

    private Expression? _expression;

    public ExpressionEvaluator(string expression)
    {
        _expressionStr = expression;
    }

    public Dictionary<string, decimal> Variables { get; } = new();

    protected double Evaluate(string expression)
    {
        _expression ??= new Expression(_expressionStr);
        _expression.
    }
}