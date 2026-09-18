using System.Collections.Generic;

namespace FullYearProject.Models.Quiz.Merging;

/// <summary>
///     Represents a mapping from a set of original topic IDs to new topic IDs.
/// </summary>
public class QuizTopicMapping
{
    // The mapping from original topic ID to new topic ID for each quiz
    private readonly Dictionary<int, int>[] _quizMappings;

    /// <summary>
    ///     The number of quizzes that this mapping is for.
    /// </summary>
    public int Count => _quizMappings.Length;

    private QuizTopicMapping(Dictionary<int, int>[] quizMappings)
    {
        _quizMappings = quizMappings;
    }

    /// <summary>
    ///     Creates a new mapping from the original topic IDs to new topic IDs where each new topic ID is unique.
    /// </summary>
    /// <param name="topicCollections">The topic collections to create a mapping of.</param>
    /// <returns>The created topic mapping.</returns>
    public static QuizTopicMapping CreateUniqueMapping(params QuizTopicCollection[] topicCollections)
    {
        var currentId = 0;

        var mappings = new Dictionary<int, int>[topicCollections.Length];

        // Add a mapping from the original topic ID to the new topic ID for each quiz, using currentIndex as the new topic ID, then incrementing
        for (var i = 0; i < topicCollections.Length; i++)
        {
            mappings[i] = new();

            foreach (var topic in topicCollections[i])
            {
                mappings[i][topic.Id] = currentId++;
            }
        }

        return new(mappings);
    }

    /// <summary>
    ///     Maps a topic ID from an existing quiz to the merged quiz.
    /// </summary>
    /// <param name="originalId">The id of the topic in the existing quiz.</param>
    /// <param name="collectionIndex">The index of the quiz that was used when generating the mapping.</param>
    /// <returns>The new topic ID.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if the value of <paramref name="originalId" /> was not found in the
    ///     mapping.
    /// </exception>
    public int MapQuizTopicId(int originalId, int collectionIndex)
    {
        var mapping = _quizMappings[collectionIndex];

        if (mapping.TryGetValue(originalId, out var mappedId))
        {
            return mappedId;
        }

        throw new KeyNotFoundException($"Could not find mapping for topic ID {originalId} in quiz 1 or quiz 2.;");
    }
}