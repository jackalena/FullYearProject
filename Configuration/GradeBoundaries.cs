using System.Linq;

namespace FullYearProject.Configuration;

/// <summary>
///     Represents a possible grade and the score range it corresponds to.
/// </summary>
/// <param name="Name">The name of the grade.</param>
/// <param name="Min">The lowest score the grade applies to.</param>
/// <param name="Max">The highest score the grade appliea to.</param>
/// <param name="Messages">A list of possible messages to show to the user if they get this grade.</param>
public record GradeBoundary(string Name, int Min, int Max, string[] Messages);

/// <summary>
///     A collection of possible grades and their corresponding score ranges.
/// </summary>
public static class GradeBoundaries
{
    /// <summary>
    ///     The possible grades and their corresponding score ranges.
    /// </summary>
    public static GradeBoundary[] Boundaries { get; } = [
        new("Not Achieved", 0, 37,
        [
            "This is just one step in learning, so take a breath and let's tackle it again!",
            "Mistakes are just proof that you are trying, so keep your head up and keep going!",
            "It didn't go your way this time, but every setback is just setup for a comeback.",
            "Don't sweat it - use this as a guide for what to practice next, you've got this!",
            "Progress takes time, so keep working hard and you'll get it next time!"
        ]),
        new("Achieved", 38, 63,
        [
            "You cleared the hurdle today, so let's keep this momentum going for the next one!",
            "You've got the basics down, and with a little more practice, you'll fly even higher!",
            "Great job passing - every step forward is building your path to success!",
            "You proved you can do it, now let's see how much further you can stretch yourself!",
            "A solid success today that shows you have exactly what it takes to keep improving!"
        ]),
        new("Merit", 64, 87,
        [
            "Fantastic job, you've shown a really strong understanding of the material!",
            "Great result - your dedication and effort are definitely paying off!",
            "Awesome work today, you are super close to the top spot!",
            "You did wonderfully well and proved you really know your stuff!",
            "Excellent effort, keep pushing yourself because you are doing great!"
        ]),
        new("Excellence", 88, 100,
        [
            "Outstanding work - you completely mastered this quiz!",
            "Brilliant effort, your hard work and focus really shine through!",
            "Incredible results, you should be incredibly proud of this achievement!",
            "Absolutely fantastic job hitting the top mark today!",
            "High-five for an amazing performance - you crushed it!"
        ])
    ];

    /// <summary>
    ///     Gets the grade for the given score.
    /// </summary>
    /// <param name="score">The score to get the grade for.</param>
    /// <returns>The calculated grade for the given score.</returns>
    public static GradeBoundary GetGrade(int score)
    {
        return Boundaries.First(b => score >= b.Min && score <= b.Max);
    }
}