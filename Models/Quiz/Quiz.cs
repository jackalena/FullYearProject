using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.Models.Quiz;

/// <summary>
///     Represents a quiz.
/// </summary>
public class Quiz
{
    /// <summary>
    ///     The settings to use for the quiz.
    /// </summary>
    public QuizSettings Settings { get; set; } = new();

    /// <summary>
    ///     The topics in the quiz.
    /// </summary>
    public QuizTopicCollection Topics { get; set; } = [];

    /// <summary>
    ///     The questions in the quiz.
    /// </summary>
    public QuizQuestionCollection Questions { get; set; } = [];

    private static JsonSerializerOptions _options = new() { UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow };

    public static async Task<Quiz> LoadFile(string filename)
    {
        await using var fileStream = File.OpenRead(filename);
        return await JsonSerializer.DeserializeAsync<Quiz>(fileStream, _options) ??
               throw new JsonException("Could not load quiz");
    }

    public static Quiz LoadString(string str)
    {
        return JsonSerializer.Deserialize<Quiz>(str, _options) ??
               throw new JsonException("Could not load quiz");
    }
}