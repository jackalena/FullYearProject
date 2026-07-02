using System.Collections.Generic;
using FullYearProject.Models.Questions;

namespace FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

public class ExpressionOptionIsCorrectDefinition : OptionIsCorrectDefinition
{
    private BooleanExpressionEvaluator? _expressionEvaluator;

    public string Expression { get; set; } = string.Empty;

    public override bool EvaluateIsCorrect(Dictionary<string, double> variables)
    {
        _expressionEvaluator ??= new(Expression);
        _expressionEvaluator.Variables = variables;
        return _expressionEvaluator.Evaluate();
    }
}