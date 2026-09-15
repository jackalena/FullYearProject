using System.Collections.Generic;
using System.Linq;

namespace FullYearProject.Models.Responses;

/// <summary>
///     A collection of question responses.
/// </summary>
public class QuestionResponseCollection : List<QuestionResponse>
{
    /// <summary>
    ///     The number of correct responses.
    /// </summary>
    public int CorrectCount => this.Count(c => c.IsAnswerCorrect);

    /// <summary>
    ///     The proportion of responses which were correct, between 0 and 1.
    /// </summary>
    public double CorrectProportion => (double) CorrectCount / Count;

    /// <inheritdoc />
    public QuestionResponseCollection() { }

    /// <inheritdoc />
    public QuestionResponseCollection(IEnumerable<QuestionResponse> collection) : base(collection) { }
}