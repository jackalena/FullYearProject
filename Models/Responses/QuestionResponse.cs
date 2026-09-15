using System;
using System.Collections.Generic;
using System.Linq;

using FullYearProject.Models.Display;

namespace FullYearProject.Models.Responses;

/// <summary>
///     Represents a response to a quiz question.
/// </summary>
public class QuestionResponse
{
    /// <summary>
    ///     The question that was answered.
    /// </summary>
    public GeneratedQuizQuestion Question { get; }

    /// <summary>
    ///     Whether the given answer is correct.
    /// </summary>
    public bool IsAnswerCorrect { get; }

    /// <summary>
    ///     The possible answers to the question and their responses.
    /// </summary>
    public QuestionOptionResponse[] Answers { get; }

    /// <summary>
    ///     Initialises a new instance of the <see cref="QuestionResponse" /> class from an existing question and a collection
    ///     of options.
    /// </summary>
    /// <param name="question">The question the response is for.</param>
    /// <param name="answers">The options present for the question.</param>
    public QuestionResponse(GeneratedQuizQuestion question, IEnumerable<QuestionOptionResponse> answers)
    {
        Question = question;
        Answers = answers.ToArray();

        IsAnswerCorrect = Array.Exists(Answers, a => a.IsSelected && a.IsCorrect.EvaluateIsCorrect(null));
    }
}