using System;
using System.Collections.Generic;
using System.Linq;
using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.Models.Responses;

/// <summary>
///     Represents a response to a quiz question.
/// </summary>
public class QuestionResponse
{
    public QuestionResponse(QuizQuestion question, IEnumerable<QuestionOptionResponse> answers)
    {
        Question = question;
        Answers = answers.ToArray();

        IsAnswerCorrect = Array.Exists(Answers, a => a.IsSelected && a.IsCorrect.EvaluateIsCorrect(null));
    }

    /// <summary>
    ///     The question that was answered.
    /// </summary>
    public QuizQuestion Question { get; }

    /// <summary>
    ///     Whether the given answer is correct.
    /// </summary>
    public bool IsAnswerCorrect { get; }

    /// <summary>
    ///     The possible answers to the question and their responses.
    /// </summary>
    public QuestionOptionResponse[] Answers { get; }
}