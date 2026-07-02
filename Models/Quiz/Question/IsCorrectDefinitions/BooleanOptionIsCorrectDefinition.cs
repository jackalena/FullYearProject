using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

public class BooleanOptionIsCorrectDefinition : OptionIsCorrectDefinition
{
    public static BooleanOptionIsCorrectDefinition Default { get; } = new() { Value = false };

    public bool Value { get; set; }

    public override bool EvaluateIsCorrect(Dictionary<string, double> variables)
    {
        return Value;
    }
}