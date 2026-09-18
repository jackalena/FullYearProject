using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Question;

/// <summary>
///     A collection of quiz questions.
/// </summary>
public class QuizQuestionCollection : List<QuizQuestion>
{
    /// <inheritdoc />
    public QuizQuestionCollection() { }

    /// <inheritdoc />
    public QuizQuestionCollection(IEnumerable<QuizQuestion> collection) : base(collection) { }

    /// <inheritdoc />
    public QuizQuestionCollection(int capacity) : base(capacity) { }
}