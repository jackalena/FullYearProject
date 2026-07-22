namespace FullYearProject.Models.Responses;

/// <summary>
///     A collection of question responses for a given topic.
/// </summary>
public class QuestionResponseTopic : QuestionResponseCollection
{
    /// <summary>
    ///     The topic of the responses.
    /// </summary>
    public string? Topic { get; set; }
}