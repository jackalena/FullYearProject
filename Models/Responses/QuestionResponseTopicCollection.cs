using System.Collections.Generic;

namespace FullYearProject.Models.Responses;

/// <summary>
///     A collection of <see cref="QuestionResponseTopic" />.
/// </summary>
public class QuestionResponseTopicCollection : List<QuestionResponseTopic>
{
    /// <inheritdoc />
    public QuestionResponseTopicCollection() { }

    /// <inheritdoc />
    public QuestionResponseTopicCollection(IEnumerable<QuestionResponseTopic> collection) : base(collection) { }
}