using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

public abstract class OptionIsCorrectDefinition
{
    public abstract bool EvaluateIsCorrect(Dictionary<string, double> variables);
}