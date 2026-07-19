using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Question.Parameters;

/// <summary>
///     Represents a parameter and its constraints that can be used in a quiz question.
/// </summary>
public class QuizQuestionParameter
{
    /// <summary>
    ///     The name of the parameter.
    /// </summary>
    public string Name { get; set; } = "Unknown";

    /// <summary>
    ///     The constraints that will be applied to the parameter when calculating a value for it.
    /// </summary>
    public List<QuizQuestionParameterConstraint> Constraints { get; set; } = [];
}