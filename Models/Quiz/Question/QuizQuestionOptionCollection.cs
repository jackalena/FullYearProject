using System.Collections.Generic;
using System.Linq;

namespace FullYearProject.Models.Quiz.Question;

/// <summary>
///     A collection of quiz question options.
/// </summary>
public class QuizQuestionOptionCollection : List<QuizQuestionOption>
{
    /// <summary>
    ///     Finds the correct option for the question.
    /// </summary>
    /// <returns>The correct option.</returns>
    public QuizQuestionOption FindCorrectOption()
    {
        return this.First(o => o.IsCorrect.EvaluateIsCorrect());
    }
}