using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Options;

/// <summary>
///     A collection of quiz question options.
/// </summary>
public class QuizQuestionOptionCollection : List<QuizQuestionOption>
{
    /// <inheritdoc />
    public QuizQuestionOptionCollection()
    {
    }

    /// <inheritdoc />
    public QuizQuestionOptionCollection(int capacity) : base(capacity)
    {
    }
}