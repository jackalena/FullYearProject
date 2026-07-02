namespace FullYearProject.Models.Questions;

/// <summary>
///     Evaluates an expression as a boolean value.
/// </summary>
/// <param name="expression">The expression to evaluate.</param>
public class BooleanExpressionEvaluator(string expression) : ExpressionEvaluator(expression)
{
    /// <summary>
    ///     Evaluate the expression as a boolean value.
    /// </summary>
    /// <returns>The evaluated value.</returns>
    public bool Evaluate()
    {
        return EvaluateInternal() != 0.0;
    }
}