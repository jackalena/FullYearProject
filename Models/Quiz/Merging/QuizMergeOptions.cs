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
    ///     Gets or sets the string to join original descriptions to create the description of the merged quiz.
    /// </summary>
    public string DescriptionSeparator { get; set; } = "\n";

    /// <summary>
    ///     Gets or sets the string to join original titles to create the title of the merged quiz.
    /// </summary>
    public string TitleSeparator { get; set; } = " + ";

    /// <summary>
    ///     Controls how the time limits of the quizzes are merged.
    /// </summary>
    public MergeTimeLimitOption TimeLimitOption { get; set; } = MergeTimeLimitOption.Average;

    /// <summary>
    ///     Gets the default set of <see cref="QuizMergeOptions" />.
    /// </summary>
    public static QuizMergeOptions Default { get; } = new();

    /// <summary>
    ///     The format to use for the topic names. The first parameter is the original topic name, the second is the original
    ///     quiz title.
    /// </summary>
    public string TopicFormat { get; set; } = "{1} - {0}";
}