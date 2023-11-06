using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIdx;
    bool hasAnsweredEarly = true;

    [Header("Buttons")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;
    public bool isComplete = false;

    /* Colors */
    Color defaultAnswerColor = new Color(1f,1f,1f,1f);
    Color correctAnswerColor = new Color(0.3537736f, 1f, 0.4059887f, 1f);
    Color wrongAnswerColor   = new Color(1f, 0.2783019f, 0.2783019f, 1f);

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void Update()
    {
        UpdateTimerImage();
        if (timer.loadNextQuestion) {
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        } else if (!hasAnsweredEarly && !timer.isAnsweringQuestion) {
            hasAnsweredEarly = true;
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++) {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    void GetNextQuestion()
    {
        if (questions.Count == 0) {
            isComplete = true;
            return;
        }

        SetButtonState(true);
        SetDefaultButtonSprites();
        GetRandomQuestion();
        DisplayQuestion();
        scoreKeeper.IncrementQuestionsSeen();
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];
        if (questions.Contains(currentQuestion)) {
            questions.Remove(currentQuestion);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    void DisplayAnswer(int index)
    {
        if (index != -1) { // Player answered
            Image buttonImage = answerButtons[index].GetComponent<Image>();

            if(index == currentQuestion.GetCorrectAnswerIndex()) {
                // Mark button selected as correct
                questionText.text = "Correto!";
                
                //buttonImage.sprite = correctAnswerSprite;
                buttonImage.color = correctAnswerColor;

                // Increment score
                scoreKeeper.IncrementCorrectAnswers();
            } else {
                // Mark button selected as wrong
                buttonImage.color = wrongAnswerColor;

                // Mark correct button
                correctAnswerIdx = currentQuestion.GetCorrectAnswerIndex();
                buttonImage = answerButtons[correctAnswerIdx].GetComponent<Image>();
                buttonImage.color = correctAnswerColor;

                string correctAnswer = currentQuestion.GetAnswer(correctAnswerIdx);
                questionText.text = "Errado :/\nResposta correta: " + correctAnswer;
            }
        } else { // Timer ran out
            // Mark correct button
            correctAnswerIdx = currentQuestion.GetCorrectAnswerIndex();
            Image buttonImage = answerButtons[correctAnswerIdx].GetComponent<Image>();
            buttonImage.color = correctAnswerColor;

            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIdx);
            questionText.text = "Acabou o tempo :/\nResposta correta: " + correctAnswer;
        }

        progressBar.value ++;
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++) {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites()
    {
        Image buttonImage;
        for (int i = 0; i < answerButtons.Length; i++) {
            buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.color = defaultAnswerColor;
        }
    }

    void UpdateTimerImage()
    {
        timerImage.fillAmount = timer.fillFraction;
    }
}
