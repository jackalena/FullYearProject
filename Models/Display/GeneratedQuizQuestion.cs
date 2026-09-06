using System;
using System.Collections.Generic;
using System.Globalization;
using FullYearProject.Helpers;
using FullYearProject.Models.Quiz.Expressions;
using FullYearProject.Models.Quiz.Options;
using FullYearProject.Models.Quiz.Question;
using FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;
using FullYearProject.Models.Responses;

namespace FullYearProject.Models.Display;

/// <summary>
///     Represents a quiz question that has been generated from another question, using the original question's parameters
///     and calculations.
/// </summary>
public class GeneratedQuizQuestion : QuizQuestion
{
    /// <summary>
    ///     Creates a new instance of the GeneratedQuizQuestion class from question data.
    /// </summary>
    /// <param name="sourceQuestion">The question the generated question is based on.</param>
    /// <param name="options">The options for the generated question.</param>
    /// <param name="text">The question text to show.</param>
    /// <param name="explanation">The question explanation.</param>
    public GeneratedQuizQuestion(QuizQuestion sourceQuestion, QuizQuestionOptionCollection options, string text,
        string? explanation)
    {
        SourceQuestion = sourceQuestion;
        Options = options;

        Text = text;
        Explanation = explanation;

        Topic = SourceQuestion.Topic;
    }

    /// <summary>
    ///     Creates a new instance of the GeneratedQuizQuestion class using question data from an existing question.
    /// </summary>
    /// <param name="sourceQuestion">The question to use data from.</param>
    public GeneratedQuizQuestion(QuizQuestion sourceQuestion) : this(sourceQuestion, sourceQuestion.Options,
        sourceQuestion.Text, sourceQuestion.Explanation)
    {
    }

    /// <summary>
    ///     The original question this question has been generated from.
    /// </summary>
    public QuizQuestion SourceQuestion { get; }

    /// <summary>
    ///     Generates a quiz question from an existing question definition.
    /// </summary>
    /// <param name="sourceQuestion">The existing question to generate based on.</param>
    /// <returns>The generated question.</returns>
    public static GeneratedQuizQuestion GenerateQuestion(QuizQuestion sourceQuestion)
    {
        // If the source question has no parameters, it is a basic text question, and we just return a GeneratedQuizQuestion using data copied from
        // the original question.
        if (sourceQuestion.Parameters == null)
        {
            if (sourceQuestion.ReuseCount > 0)
            {
                return new ReusableGeneratedQuizQuestion(sourceQuestion);
            }

            return new(sourceQuestion);
        }

        Dictionary<string, double> parameters = new();

        // Evaluate the value of each of the question's parameters.
        foreach (var parameter in sourceQuestion.Parameters)
        {
            parameters[parameter.Name] = 0;

            // Apply each constraint to the parameter and update it with its new value.
            foreach (var constraint in parameter.Constraints)
            {
                constraint.Variables = parameters;
                parameters[parameter.Name] = constraint.Apply(parameters[parameter.Name]);
            }
        }

        // Format the text in the question to include the question's parameters.
        var formatter = new StringVariableFormatter(parameters);

        var questionText = formatter.Format(sourceQuestion.Text);
        var questionExplanation = sourceQuestion.Explanation?.Apply(formatter.Format);

        var questionOptions = new QuizQuestionOptionCollection(sourceQuestion.Options.Count);

        // Generate each option for the question
        foreach (var option in sourceQuestion.Options)
        {
            string? optionText = null;

            switch (option)
            {
                // If the option is plain text, use this text with the question parameters
                case TextQuizQuestionOption textOption:
                    optionText = formatter.Format(textOption.Value);

                    break;

                // If the question is an expression, evaluate its value and use this value as the option.
                case ExpressionQuizQuestionOption expressionOption:
                    NumericalExpressionEvaluator evaluator = new(expressionOption.Value) { Variables = parameters };

                    var optionValue = evaluator.Evaluate();

                    if (expressionOption.Format != null)
                    {
                        try
                        {
                            optionText = string.Format(expressionOption.Format, optionValue);
                        }
                        catch (FormatException)
                        {
                            optionText = null;
                        }
                    }

                    // If the format couldn't be applied, just display the calculated value.
                    optionText ??= optionValue.ToString(CultureInfo.CurrentCulture);

                    break;
                default:

                    // Fall back to show the original option text if the option is some other type.
                    optionText = option.Value;

                    break;
            }

            // Evaluate whether the option is correct.
            var isCorrectDef = new BooleanOptionIsCorrectDefinition
                { Value = option.IsCorrect.EvaluateIsCorrect(parameters) };

            // Add the generated option to the list of the question's options.
            questionOptions.Add(new TextQuizQuestionOption { Value = optionText, IsCorrect = isCorrectDef });
        }

        // Randomise the order of the question's options.
        questionOptions.Shuffle();

        if (sourceQuestion.ReuseCount > 0)
        {
            return new ReusableGeneratedQuizQuestion(sourceQuestion, questionOptions, questionText,
                questionExplanation);
        }

        return new(sourceQuestion, questionOptions, questionText, questionExplanation);
    }
}

/// <inheritdoc />
/// Supports showing the question multiple times.
public class ReusableGeneratedQuizQuestion : GeneratedQuizQuestion, IReusablePrioritisedListItem
{
    /// <inheritdoc />
    public ReusableGeneratedQuizQuestion(QuizQuestion sourceQuestion, QuizQuestionOptionCollection options, string text,
        string? explanation) :
        base(sourceQuestion, options, text, explanation)
    {
    }

    /// <inheritdoc />
    public ReusableGeneratedQuizQuestion(QuizQuestion sourceQuestion) : base(sourceQuestion)
    {
    }

    /// <inheritdoc />
    public int CurrentReuseCount { get; set; }
}