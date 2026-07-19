namespace FullYearProject.Models.Quiz;

/// <summary>
///     Represents a quiz topic.
/// </summary>
public class QuizTopic
{
    /// <summary>
    ///     The id of the topic.
    /// </summary>
    public int Id { get; set; } = -1;

    /// <summary>
    ///     The name of the topic.
    /// </summary>
    public string Name { get; set; } = "Topic";

    /// <summary>
    ///     A description of the topic.
    /// </summary>
    public string? Description { get; set; }
}