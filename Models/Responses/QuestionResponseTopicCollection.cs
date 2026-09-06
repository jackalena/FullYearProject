using System.Collections.Generic;

namespace FullYearProject.Models.Responses;

/// <summary>
///     A collection of <see cref="QuestionResponseTopic" />.
/// </summary>
public class QuestionResponseTopicCollection : List<QuestionResponseTopic>
{
    public QuestionResponseTopicCollection()
    {
    }

    public QuestionResponseTopicCollection(IEnumerable<QuestionResponseTopic> collection) : base(collection)
    {
    }
}