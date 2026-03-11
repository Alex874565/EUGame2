using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(SolvableObjectSFX))]
public class QuizMinigameController : MinigameController
{
    [SerializeField] private MenuStaggerAnimation stagger;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private List<Button> _answerButtons;
    
    private List<QuizData> _questions;
    private QuizData _quizData;

    private SolvableObjectSFX _objectSfx;

    private bool answering;
    
    private new void Awake()
    {
        base.Awake();
        _objectSfx = GetComponent<SolvableObjectSFX>();
    }

    /*private void Start()
    {
        Debug.Log("starting minigame");
        StartMinigame();
    }*/

    private new void Update()
    {
        base.Update();
    }

    public override void StartMinigame()
    {
        base.StartMinigame();

        _questions = new List<QuizData>(ServiceLocator.Instance.QuizzesDatabase.Quizzes);

        Debug.Log("before stagger");

        stagger.OpenMenu();
        InitializeQuestion();
    }

    private void GetRandomQuestion()
    {
        if(_questions.Count == 0)
        {
            _questions = new List<QuizData>(ServiceLocator.Instance.QuizzesDatabase.Quizzes);
        }
        
        int index = Random.Range(0, _questions.Count);
        
        _quizData = _questions[index];
        _questions.RemoveAt(index);
    }

    private void InitializeQuestion()
    {
        GetRandomQuestion();

        answering = false;

        foreach (var btn in _answerButtons)
        {
            btn.interactable = true;

            var text = btn.GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.white;
        }

        _questionText.text = _quizData.Question;
        List<QuizAnswerData> answers = _quizData.GetRandomizedAnswers();
        for(int i = 0; i < _answerButtons.Count; i++)
        {
            if(i >= answers.Count)
            {
                _answerButtons[i].gameObject.SetActive(false);
            }
            else
            {
                QuizAnswerData answer = answers[i];
                Button button = _answerButtons[i];

                button.gameObject.SetActive(true);
                button.GetComponentInChildren<TextMeshProUGUI>().text = answer.Text;

                button.onClick.RemoveAllListeners();

                bool isCorrect = answer.IsCorrect;

                button.onClick.AddListener(() => AnswerQuestion(isCorrect, button));
            }
        }
    }

    private void AnswerQuestion(bool isCorrect, Button clickedButton)
    {
        if (answering) return;
        answering = true;

        if (isCorrect)
            _objectSfx.PlaySolveSFX();
        else
            _objectSfx.PlayUnsolveSFX();

        AddScore(isCorrect ? 1 : 0);

        // Disable all buttons
        foreach (var btn in _answerButtons)
            btn.interactable = false;

        ShowAnswerFeedback(clickedButton);

        Invoke(nameof(CloseForNextQuestion), 0.8f);
    }

    private void ShowAnswerFeedback(Button clickedButton)
    {
        foreach (var btn in _answerButtons)
        {
            var text = btn.GetComponentInChildren<TextMeshProUGUI>();

            bool isCorrectAnswer =
                _quizData.Answers.Exists(a => a.Text == text.text && a.IsCorrect);

            if (isCorrectAnswer)
            {
                // Correct answer = green + pop
                text.color = Color.green;

                btn.transform
                    .DOPunchScale(Vector3.one * 0.25f, 0.4f)
                    .SetUpdate(true);
            }
            else if (btn == clickedButton)
            {
                // Wrong clicked answer = red + shake
                text.color = Color.red;

                btn.transform
                    .DOShakePosition(0.4f, 10f, 20)
                    .SetUpdate(true);
            }
        }
    }

    private void CloseForNextQuestion()
    {
        stagger.CloseMenu(() =>
        {
            InitializeQuestion();
            stagger.OpenMenu();
        });
    }
}