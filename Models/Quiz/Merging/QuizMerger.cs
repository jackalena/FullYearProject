using System;

namespace FullYearProject.Models.Quiz.Merging;

/// <summary>
///     Defines options for merging quizzes.
/// </summary>
public class QuizMergeOptions
{
    /// <summary>
    ///     Defines how to merge the time limits of the quizzes.
    /// </summary>
    public enum MergeTimeLimitOption
    {
        /// <summary>
        ///     Use the first quiz's time limit.
        /// </summary>
        First,

        /// <summary>
        ///     Use the sum of the time limits of all quizzes.
        /// </summary>
        Sum,

        /// <summary>
        ///     Use the average of the time limits of all quizzes.
        /// </summary>
        Average
    }

    /// <summary>
    ///     Gets or sets the format string to create the description of the merged quiz.
    /// </summary>
    public FormattableString DescriptionFormat
    {
        get;
        set
        {
            if (value.ArgumentCount != field.ArgumentCount)
            {
                throw new ArgumentException(
                    $"Format must have exactly {field.ArgumentCount} arguments.",
                    nameof(value));
            }

            field = value;
        }
    } = $"{0}\n{1}";

    /// <summary>
    ///     Gets or sets the format string to create the title of the merged quiz.
    /// </summary>

    public FormattableString TitleFormat
    {
        get;
        set
        {
            if (value.ArgumentCount != field.ArgumentCount)
            {
                throw new ArgumentException($"Format must have exactly {field.ArgumentCount} arguments.",
                    nameof(value));
            }

            field = value;
        }
    } = $"{0} + {1}";

    /// <summary>
    ///     Controls how the time limits of the quizzes are merged.
    /// </summary>
    public MergeTimeLimitOption TimeLimitOption { get; set; } = MergeTimeLimitOption.Average;

    /// <summary>
    ///     Gets the default set of <see cref="QuizMergeOptions" />.
    /// </summary>
    public static QuizMergeOptions Default { get; } = new();
}

/// <summary>
///     Provides methods for merging quizzes.
/// </summary>
public static class QuizMerger
{
    public static Quiz Merge(Quiz quiz1, Quiz quiz2, QuizMergeOptions? options)
    {
        options ??= QuizMergeOptions.Default;

        Quiz quiz = new()
        {
            Settings = new()
        };


        throw new NotImplementedException();
    }
}