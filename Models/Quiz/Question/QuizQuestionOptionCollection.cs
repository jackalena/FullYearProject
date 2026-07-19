using System;
using System.Collections.Generic;

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
        throw new NotImplementedException();
    }
}