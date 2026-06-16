using System.Collections.Generic;
using System.Linq;

namespace FullYearProject.Models.Questions;

public class QuestionResultCollection : List<QuestionResult>
{
    public int CorrectCount => this.Count(c => c.IsCorrect);
}