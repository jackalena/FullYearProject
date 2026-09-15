using System.Collections.Generic;

using Microsoft.Extensions.Logging;

namespace FullYearProject.Models.Quiz;

/// <summary>
///     A collection of quiz topics.
/// </summary>
public class QuizTopicCollection : List<QuizTopic>
{
    /// <summary>
    ///     An <see cref="ILogger" /> instance for the class.
    ///     Only created if accessed.
    /// </summary>
    protected ILogger Logger => field ??= Logging.Logger.Create(GetType());

    private Dictionary<int, QuizTopic> TopicsById => field ??= CreateDict();

    private Dictionary<int, QuizTopic> CreateDict()
    {
        var dict = new Dictionary<int, QuizTopic>(Count);

        foreach (var topic in this)
        {
            if (!dict.TryAdd(topic.Id, topic))
            {
                Logger.LogError("Duplicate topic ID: {Id}", topic.Id);
            }
        }

        return dict;
    }

    /// <summary>
    ///     Gets a topic by its ID.
    /// </summary>
    /// <param name="topicId">The ID of the topic.</param>
    /// <returns>The QuizTopic object with the provided ID.</returns>
    public QuizTopic FromId(int topicId)
    {
        return TopicsById[topicId];
    }
}