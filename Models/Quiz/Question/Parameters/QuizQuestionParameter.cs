using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Question.Parameters;

public class QuizQuestionParameter
{
    public string Name { get; set; } = "Unknown";
    public List<QuizQuestionParameterConstraint> Constraints { get; set; } = [];
}