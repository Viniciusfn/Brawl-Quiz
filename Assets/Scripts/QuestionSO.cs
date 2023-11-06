using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz Question", order = 0)]
public class QuestionSO : ScriptableObject {

    [TextArea(2,4)] [SerializeField] string question = "Enter your new question here?";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIdx;

    public string GetQuestion()
    {
        return question;
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIdx;
    }

    public string GetAnswer(int index)
    {
        return answers[index];
    }
}
