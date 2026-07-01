using org.mariuszgromada.math.mxparser;

namespace FullYearProject.Models.Questions;

public class MathExpression
{
    private string? _evaluatedExpression;

    public MathExpression(string expression)
    {
        Expression = new Expression(expression);
    }

    public string SourceExpression
    {
        get => Expression.getExpressionString();
        set => Expression.setExpressionString(value);
    }

    public string EvaluatedExpression => _evaluatedExpression ??= EvaluatedExpression;
    internal Expression Expression { get; set; }


    private void EvaluateExpression()
    {
    }
}