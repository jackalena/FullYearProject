using Avalonia.Controls;
using FullYearProject.Models.Quiz.Question;

namespace FullYearProject.Models;

public static class DesignQuiz
{
    #region SampleQuizData

    private static readonly string? QuizText =
#if !DEBUG
null;
#else
        Design.IsDesignMode
            ? null
            : """
              {
                  "Settings": {
                      "Title": "Living Labs",
                      "TimeLimit": "00:30:00"
                  },
                  "Topics": [
                      {
                          "Id": 0,
                          "Name": "MRS C GREN"
                      },
                      {
                          "Id": 1,
                          "Name": "Cellular Respiration"
                      },
                      {
                          "Id": 2,
                          "Name": "Cells"
                      }
                  ],
                  "Questions": [
                      {
                          "Topic": 0,
                          "Name": "True or false: All living organisms carry out the eight life processes known as MRS C GREN.",
                          "Options": [
                              {
                                  "Type": "Text",
                                  "Value": "True",
                                  "IsCorrect": true
                              },
                              {
                                  "Type": "Text",
                                  "Value": "False"
                              }
                          ],
                          "Explanation": "MRS C GREN stands for Movement, Respiration, Sensitivity, Cells, Growth, Reproduction, Excretion, Nutrition - every living organism does all eight."
                      },
                      {
                          "Topic": 0,
                          "Name": "True or false: Fire meets all eight criteria of MRS C GREN and should be classified as a living organism.",
                          "Options": [
                              {
                                  "Type": "Text",
                                  "Value": "True"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "False",
                                  "IsCorrect": true
                              }
                          ],
                          "Explanation": "Fire only meets about 4 of the 8 criteria (movement, sensitivity, and some growth/reproduction). It has no cells, doesn't truly respire, excrete, or get nutrition."
                      },
                      {
                          "Topic": 0,
                          "Name": "Which letter in MRS C GREN stands for the removal of waste products made inside the body's cells?",
                          "Options": [
                              {
                                  "Type": "Text",
                                  "Value": "R - Respiration"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "E - Excretion",
                                  "IsCorrect": true
                              },
                              {
                                  "Type": "Text",
                                  "Value": "N - Nutrition"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "S - Sensitivity"
                              }
                          ],
                          "Explanation": "E stands for Excretion - the removal of metabolic waste products produced inside cells, such as exhaling CO₂ or sweating."
                      },
                      {
                          "Topic": 0,
                          "Name": "True or false: Plants **do not** carry out all eight MRS C GREN life processes because they cannot move.",
                          "Options": [
                              {
                                  "Type": "Text",
                                  "Value": "True"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "False",
                                  "IsCorrect": true
                              }
                          ],
                          "Explanation": "Plants do carry out all eight processes. They exhibit localised movement (e.g., growing toward light) even though they don't move their whole body from place to place."
                      },
                      {
                          "Topic": 0,
                          "Name": "True or false: Water scores 2 out of 8 on the MRS C GREN criteria.",
                          "Options": [
                              {
                                  "Type": "Text",
                                  "Value": "True",
                                  "IsCorrect": true
                              },
                              {
                                  "Type": "Text",
                                  "Value": "False"
                              }
                          ],
                          "Explanation": "Water can show movement and sensitivity but fails the other six criteria (no cells, no respiration, no growth, no reproduction, no excretion, no nutrition)."
                      },
                      {
                          "Topic": 1,
                          "Name": "True or false: Cellular respiration only happens when an organism is physically active.",
                          "Options": [
                              {
                                  "Type": "Text",
                                  "Value": "True"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "False",
                                  "IsCorrect": true
                              }
                          ],
                          "Explanation": "Cellular respiration takes place constantly in every cell of every living organism. If a cell stops respiring it will eventually die."
                      },
                      {
                          "Topic": 1,
                          "Name": "What is the correct word equation for cellular respiration?",
                          "Options": [
                              {
                                  "Type": "Text",
                                  "Value": "$\\text{Glucose} + \\text{oxygen} \\rightarrow \\text{carbon dioxide} + \\text{water} + \\text{energy}$",
                                  "IsCorrect": true
                              },
                              {
                                  "Type": "Text",
                                  "Value": "$\\text{Carbon dioxide} + \\text{water} \\rightarrow \\text{glucose} + \\text{oxygen}$"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "$\\text{Glucose} + \\text{carbon dioxide} \\rightarrow \\text{oxygen} + \\text{water} + \\text{energy}$"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "$\\text{Oxygen} + \\text{water} \\rightarrow \\text{glucose} + \\text{carbon dioxide}$"
                              }
                          ],
                          "Explanation": "$\\text{Glucose} + \\text{oxygen} \\rightarrow \\text{carbon dioxide} + \\text{water} + \\text{energy}$. This is the opposite of photosynthesis."
                      },
                      {
                          "Topic": 1,
                          "Name": "True or false: Bacteria carry out cellular respiration only in their mitochondria.",
                          "Options": [
                              {
                                  "Type": "Text",
                                  "Value": "True"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "False",
                                  "IsCorrect": true
                              }
                          ],
                          "Explanation": "Bacteria don't have mitochondria. Their cellular respiration takes place only in the cytoplasm."
                      },
                      {
                          "Topic": 1,
                          "Name": "True or false: Photosynthesis (not cellular respiration) provides energy for life processes in plants.",
                          "Options": [
                              {
                                  "Type": "Text",
                                  "Value": "True"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "False",
                                  "IsCorrect": true
                              }
                          ],
                          "Explanation": "Photosynthesis creates glucose, but it is cellular respiration that breaks down that glucose to provide energy for life processes."
                      },
                      {
                          "Topic": 1,
                          "Name": "In which part of an animal cell does cellular respiration mainly take place?",
                          "Options": [
                              {
                                  "Type": "Text",
                                  "Value": "Nucleus"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "Cell membrane"
                              },
                              {
                                  "Type": "Text",
                                  "Value": "Mitochondria and cytoplasm",
                                  "IsCorrect": true
                              },
                              {
                                  "Type": "Text",
                                  "Value": "Chloroplast"
                              }
                          ],
                          "Explanation": "Cellular respiration in animal cells mainly takes place inside the mitochondria and in the cytoplasm."
                      } 
                  ]
              }
              """;
#endif

    #endregion

    public static Quiz.Quiz? Quiz => field ??= GenerateQuiz();

    public static QuizQuestion? Question => Quiz?.Questions[0];

    private static Quiz.Quiz? GenerateQuiz()
    {
        return QuizText == null ? null : Models.Quiz.Quiz.LoadString(QuizText);
    }
}