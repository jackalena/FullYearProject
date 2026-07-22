namespace FullYearProject.Models.Quiz.Expressions;

/// <summary>
///     Evaluates a numerical expression as a number.
/// </summary>
/// <param name="expression">
///     <inheritdoc cref="ExpressionEvaluator(string)" path="/param[@name='expression']" />
/// </param>
public class NumericalExpressionEvaluator(string expression) : ExpressionEvaluator(expression)
{
    /// <summary>
    ///     Evaluate the expression as a number.
    /// </summary>
    /// <returns>The evaluated number, as a double.</returns>
    public double Evaluate()
    {
        return EvaluateInternal();
    }
}