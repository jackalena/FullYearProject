using System;
using System.Linq;

using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.Models.Quiz.Merging;

/// <summary>
///     Provides methods for merging quizzes.
/// </summary>
public static class QuizMerger
{
    /// <summary>
    ///     Calculate the time limit of the merged quiz.
    /// </summary>
    /// <param name="options">The set of options to use when joining quizzes.</param>
    /// <param name="quizzes">The set of quizzes to merge the time limits of.</param>
    /// <returns>The calculated time limit to use for the merged quiz.</returns>
    /// <exception cref="ArgumentException">Thrown if no quizzes are provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the value of <see cref="QuizMergeOptions.TimeLimitOption" /> in
    ///     <paramref name="options" /> is invalid.
    /// </exception>
    public static TimeSpan CalculateTimeLimit(QuizMergeOptions options, params Quiz[] quizzes)
    {
        if (quizzes.Length < 1)
        {
            throw new ArgumentException("At least one quiz must be provided.", nameof(quizzes));
        }

        return options.TimeLimitOption switch {
            QuizMergeOptions.MergeTimeLimitOption.First => quizzes[0].Settings.TimeLimit,
            QuizMergeOptions.MergeTimeLimitOption.Sum   => quizzes.Aggregate(TimeSpan.Zero, (total, quiz) => total + quiz.Settings.TimeLimit),
            QuizMergeOptions.MergeTimeLimitOption.Average => quizzes.Aggregate(TimeSpan.Zero, (total, quiz) => total + quiz.Settings.TimeLimit) /
                                                             quizzes.Length,
            _ => throw new ArgumentOutOfRangeException(nameof(options), options, "Invalid value for QuizMergeOptions.TimeLimitOption")
        };
    }

    /// <summary>
    ///     Merges multiple topic collections from a set of Quizzes into a single collection.
    /// </summary>
    /// <param name="mapping">The topic mapping to use.</param>
    /// <param name="options">The set of options to use when merging the topics.</param>
    /// <param name="quizCollections">The collections of quizzes containing the topics to merge.</param>
    /// <returns>A collection of the merged topics.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the number of collections in the provided mapping doesn't match the
    ///     number of provided collections.
    /// </exception>
    public static QuizTopicCollection MergeTopics(QuizTopicMapping mapping, QuizMergeOptions options, params Quiz[] quizCollections)
    {
        if (mapping.Count != quizCollections.Length)
        {
            throw new ArgumentException("The number of mappings must match the number of topic collections.", nameof(quizCollections));
        }

        QuizTopicCollection merged = new(quizCollections.Sum(c => c.Topics.Count));

        // Add a copy of each topic to the merged collection, mapping the original topic IDs to the new topic IDs
        for (var i = 0; i < quizCollections.Length; i++)
        {
            merged.AddRange(quizCollections[i]
                           .Topics
                           .Select(topic => new QuizTopic {
                                Id = mapping.MapQuizTopicId(topic.Id, i),
                                Name = string.Format(options.TopicFormat, topic.Name, quizCollections[i].Settings.Title),
                                Description = topic.Description
                            }));
        }

        return merged;
    }

    /// <summary>
    ///     Merges multiple question collections into a single collection.
    /// </summary>
    /// <param name="mapping">The topic mapping to use.</param>
    /// <param name="options">The set of options to use when merging the questions.</param>
    /// <param name="questionCollections">The collections of questions to merge.</param>
    /// <returns>A collection of the merged questions.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the number of collections in the provided mapping doesn't match the
    ///     number of provided collections.
    /// </exception>
    public static QuizQuestionCollection MergeQuestions(QuizTopicMapping mapping, QuizMergeOptions options,
                                                        params Quiz[] questionCollections)
    {
        if (mapping.Count != questionCollections.Length)
        {
            throw new ArgumentException("The number of mappings must match the number of question collections.", nameof(questionCollections));
        }

        QuizQuestionCollection merged = new(questionCollections.Sum(c => c.Questions.Count));

        // Add a copy of each question to the merged collection, mapping the original topic IDs to the new topic IDs
        for (var i = 0; i < questionCollections.Length; i++)
        {
            merged.AddRange(questionCollections[i]
                           .Questions
                           .Select(question =>
                            {
                                question.ShallowClone();
                                question.Topic = mapping.MapQuizTopicId(question.Topic, i);

                                return question;
                            }));
        }

        return merged;
    }

    /// <summary>
    ///     Merges two quizzes into a new quiz containing all the questions from both quizzes.
    /// </summary>
    /// <param name="options">The options to use to join the quizzes. Can be null to use the default options.</param>
    /// <param name="quizzes">The set of quizzes to merge into one.</param>
    /// <returns>The merged quiz.</returns>
    public static Quiz Merge(QuizMergeOptions? options = null, params Quiz[] quizzes)
    {
        // Use default options if none were provided
        options ??= QuizMergeOptions.Default;

        // Creates a mapping from the original topic IDs to the new topic IDs so no ids are used by multiple topics
        var topicMapping = QuizTopicMapping.CreateUniqueMapping(quizzes.Select(q => q.Topics).ToArray());

        // Create a new quiz with the merged topics and questions
        Quiz quiz = new() {
            Settings = new() {
                TimeLimit = CalculateTimeLimit(options, quizzes),
                Title = string.Join(options.TitleSeparator, quizzes.Select(q => q.Settings.Title)),
                Description = string.Join(options.DescriptionSeparator, quizzes.Select(q => q.Settings.Description)),
                FileName = null
            },
            Topics = MergeTopics(topicMapping, options, quizzes),
            Questions = MergeQuestions(topicMapping, options, quizzes)
        };

        return quiz;
    }
}