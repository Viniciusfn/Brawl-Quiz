using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float timerValue = 0;
    public bool isAnsweringQuestion = false;
    public float fillFraction;
    public bool loadNextQuestion = false;

    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 5f;

    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (timerValue <= 0) { // Timer ran out
            if (isAnsweringQuestion) {
                timerValue = timeToShowCorrectAnswer;
                isAnsweringQuestion = false;
            } else {
                timerValue = timeToCompleteQuestion;
                isAnsweringQuestion = true;
                loadNextQuestion = true;
            }
        } else { // Time still running
            if (isAnsweringQuestion) {
                fillFraction = timerValue / timeToCompleteQuestion;
            } else {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }
        }
    }

    public void CancelTimer()
    {
        timerValue = 0;
    }
}
