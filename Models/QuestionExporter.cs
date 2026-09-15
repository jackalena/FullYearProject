using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Platform.Storage;

using FullYearProject.Logging;
using FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

using Microsoft.Extensions.Logging;

namespace FullYearProject.Models;

/// <summary>
///     Class to export quizzes to Markdown files.
/// </summary>
public class QuestionExporter : Loggable
{
    /// <summary>
    ///     Exports a quiz to a Markdown file, using input from the user for the quiz file and output folder locations.
    /// </summary>
    /// <param name="topLevel">The top level control containing an <see cref="IStorageProvider" />.</param>
    public async Task ExportQuestion(TopLevel topLevel)
    {
        var inputFiles = await GetInputFiles(topLevel);

        if (inputFiles == null)
        {
            Logger.LogWarning("No files selected as input.");

            return;
        }

        var outputFolder = await GetOutputFolder(topLevel);

        if (outputFolder == null)
        {
            Logger.LogWarning("No output folder selected.");

            return;
        }

        // Export each quiz to a markdown file, processing files in parallel.
        await Parallel.ForEachAsync(inputFiles, async (inFile, _) =>
        {
            await using var inStream = await inFile.OpenReadAsync();

            var outFileName = Path.ChangeExtension(inFile.Name, ".md");
            var i = 1;
            while (await outputFolder.GetFileAsync(outFileName) != null)
            {
                outFileName = Path.ChangeExtension(inFile.Name, $"({i++}).md");
            }

            var outFile = await outputFolder.CreateFileAsync(outFileName);
            if (outFile == null)
            {
                Logger.LogError("Could not create output file");

                return;
            }

            var outStream = await outFile.OpenWriteAsync();

            var quiz = await Quiz.Quiz.LoadStreamAsync(inStream, inFile.Name);

            await ExportQuizAsync(quiz, outStream);
        });
    }

    private async Task<IReadOnlyList<IStorageFile>?> GetInputFiles(TopLevel topLevel)
    {
        var op = new FilePickerOpenOptions {
            Title = "Open quiz file",
            AllowMultiple = true,
            FileTypeFilter = [new("JSON Files") { Patterns = ["*.json"] }, new("All Files") { Patterns = ["*.*"] }]
        };

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(op);

        return files.Count > 0 ? files : null;
    }

    private async Task<IStorageFolder?> GetOutputFolder(TopLevel topLevel)
    {
        var op = new FolderPickerOpenOptions { Title = "Select output folder", AllowMultiple = false };

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(op);

        return folders.Count > 0 ? folders[0] : null;
    }

    // Exports a quiz to a markdown file.
    private async Task ExportQuizAsync(Quiz.Quiz quiz, Stream stream)
    {
        await using var writer = new StreamWriter(stream);

        // Write the quiz title and description.
        await writer.WriteLineAsync($"\n# {quiz.Settings.Title}");
        if (quiz.Settings.Description != null)
        {
            await writer.WriteLineAsync($"\n## {quiz.Settings.Description}");
        }

        await writer.WriteLineAsync();

        // Write each topic and its questions.
        var groupedQuestions = quiz.Questions.GroupBy(q => q.Topic);

        foreach (var topicGroup in groupedQuestions)
        {
            var topic = quiz.Topics.FromId(topicGroup.Key);
            await writer.WriteLineAsync($"## {topic.Name}");
            if (topic.Description != null)
            {
                await writer.WriteLineAsync($"\n*{topic.Description}*");
            }

            foreach (var question in topicGroup)
            {
                await writer.WriteLineAsync($"\n### {question.Text}\n");

                var correctAnswer =
                    question.Options.FirstOrDefault(o => o.IsCorrect is BooleanOptionIsCorrectDefinition { Value: true }, question.Options[0]);

                await writer.WriteLineAsync($"Correct answer: {correctAnswer.Value}");
            }
        }
    }
}