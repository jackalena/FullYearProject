using System.Collections.Generic;

using FullYearProject.Models.Quiz;

namespace FullYearProject.Models.Responses;

/// <summary>
///     A collection of question responses for a given topic.
/// </summary>
public class QuestionResponseTopic : QuestionResponseCollection
{
    /// <summary>
    ///     The topic of the responses.
    /// </summary>
    public QuizTopic? Topic { get; set; }

    /// <inheritdoc />
    public QuestionResponseTopic() { }

    /// <inheritdoc />
    public QuestionResponseTopic(IEnumerable<QuestionResponse> collection) : base(collection) { }
}