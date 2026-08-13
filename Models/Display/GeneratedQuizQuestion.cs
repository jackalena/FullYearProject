using System.Collections.Generic;
using FullYearProject.Helpers;
using FullYearProject.Models.Quiz.Expressions;
using FullYearProject.Models.Quiz.Options;
using FullYearProject.Models.Quiz.Question;
using FullYearProject.Models.Quiz.Question.IsCorrectDefinitions;

namespace FullYearProject.Models.Display;

public class GeneratedQuizQuestion : QuizQuestion
{
    public GeneratedQuizQuestion(QuizQuestion sourceQuestion, QuizQuestionOptionCollection options, string text,
        string? explanation)
    {
        SourceQuestion = sourceQuestion;
        Options = options;

        Text = text;
        Explanation = explanation;

        Topic = SourceQuestion.Topic;
    }

    public GeneratedQuizQuestion(QuizQuestion sourceQuestion) : this(sourceQuestion, sourceQuestion.Options,
        sourceQuestion.Text, sourceQuestion.Explanation)
    {
    }

    public QuizQuestion SourceQuestion { get; }

    public static GeneratedQuizQuestion GenerateQuestion(QuizQuestion sourceQuestion)
    {
        if (sourceQuestion.Parameters == null)
        {
            return new(sourceQuestion);
        }

        Dictionary<string, double> parameters = new();

        foreach (var parameter in sourceQuestion.Parameters)
        {
            parameters[parameter.Name] = 0;

            foreach (var constraint in parameter.Constraints)
            {
                constraint.Variables = parameters;
                constraint.Apply(parameters[parameter.Name]);
            }
        }

        var formatter = new StringVariableFormatter(parameters);

        var questionText = formatter.Format(sourceQuestion.Text);
        var questionExplanation = sourceQuestion.Explanation?.Apply(formatter.Format);

        var questionOptions = new QuizQuestionOptionCollection(sourceQuestion.Options.Count);

        foreach (var option in sourceQuestion.Options)
        {
            string optionText;

            switch (option)
            {
                case TextQuizQuestionOption textOption:
                    optionText = formatter.Format(textOption.Value);
                    break;
                case ExpressionQuizQuestionOption expressionOption:
                    NumericalExpressionEvaluator evaluator = new(expressionOption.Value)
                    {
                        Variables = parameters
                    };

                    var optionValue = evaluator.Evaluate();

                    optionText = optionValue.ToString(expressionOption.Format);
                    break;
                default:
                    optionText = option.Value;
                    break;
            }

            var isCorrectDef = new BooleanOptionIsCorrectDefinition
                { Value = option.IsCorrect.EvaluateIsCorrect(parameters) };

            questionOptions.Add(new TextQuizQuestionOption
            {
                Value = optionText,
                IsCorrect = isCorrectDef
            });
        }

        return new(sourceQuestion, questionOptions, questionText, questionExplanation);
    }
}