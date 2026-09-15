using System;
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
    private static JsonSerializerOptions _options = new() { UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow };

    /// <summary>
    ///     The settings to use for the quiz.
    /// </summary>
    public QuizSettings Settings { get; init; } = new();

    /// <summary>
    ///     The topics in the quiz.
    /// </summary>
    public QuizTopicCollection Topics { get; init; } = [];

    /// <summary>
    ///     The questions in the quiz.
    /// </summary>
    public QuizQuestionCollection Questions { get; init; } = [];

    /// <summary>
    ///     Loads a quiz from a file.
    /// </summary>
    /// <param name="filename">The path to the file containing the quiz data.</param>
    /// <returns>The loaded quiz.</returns>
    /// <exception cref="JsonException">Thrown if the file's JSON could not be read.</exception>
    public static async Task<Quiz> LoadFileAsync(string filename)
    {
        await using var fileStream = File.OpenRead(filename);

        return await LoadStreamAsync(fileStream, filename);
    }

    /// <summary>
    ///     Loads a quiz from a stream. This method does not close the stream.
    /// </summary>
    /// <param name="stream">The stream containing the quiz data.</param>
    /// <param name="filename">The optional name of the file containing the quiz.</param>
    /// <returns>The loaded quiz.</returns>
    /// <exception cref="JsonException">Thrown if the file's JSON could not be read.</exception>
    public static async Task<Quiz> LoadStreamAsync(Stream stream, string? filename = null)
    {
        var quiz = await JsonSerializer.DeserializeAsync<Quiz>(stream, _options);

        if (quiz == null)
        {
            throw new JsonException("Could not load quiz");
        }

        quiz.Settings.FileName = filename;

        return quiz;
    }

    /// <summary>
    ///     Loads only the quiz settings from a quiz data file.
    /// </summary>
    /// <param name="filename">The path to the file.</param>
    /// <returns>The loaded quiz settings.</returns>
    public static async Task<QuizSettings> LoadSettingsAsync(string filename)
    {
        await using var fileStream = File.OpenRead(filename);

        // Only read the settings section of the file, much faster than deserializing the whole quiz.
        using var doc = await JsonDocument.ParseAsync(fileStream);

        var root = doc.RootElement;

        if (root.TryGetProperty(nameof(Settings), out var settingsJson))
        {
            var quizSettings = JsonSerializer.Deserialize<QuizSettings>(settingsJson.GetRawText(), _options);

            if (quizSettings == null)
            {
                throw new JsonException("Could not load quiz settings");
            }

            quizSettings.FileName = filename;

            return quizSettings;
        }

        throw new FormatException("Could not find quiz settings in file");
    }
}