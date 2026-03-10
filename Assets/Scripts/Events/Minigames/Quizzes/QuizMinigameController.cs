using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(SolvableObjectSFX))]
public class QuizMinigameController : MinigameController
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private List<Button> _answerButtons;
    
    private List<QuizData> _questions;
    private QuizData _quizData;

    private SolvableObjectSFX _objectSfx;
    
    private new void Awake()
    {
        base.Awake();
        _objectSfx = GetComponent<SolvableObjectSFX>();
    }
    
    private new void Update()
    {
        base.Update();
    }

    public override void StartMinigame()
    {
        base.StartMinigame();
        _questions = new List<QuizData>(ServiceLocator.Instance.QuizzesDatabase.Quizzes);
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
                _answerButtons[i].gameObject.SetActive(true);
                _answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answer.Text;
                _answerButtons[i].onClick.RemoveAllListeners();
                _answerButtons[i].onClick.AddListener(() => AnswerQuestion(answer.IsCorrect));
            }
        }
    }

    private void AnswerQuestion(bool isCorrect)
    {
        if (isCorrect)
        {
            _objectSfx.PlaySolveSFX();
        }
        else
        {
            _objectSfx.PlayUnsolveSFX();
        }
        AddScore(isCorrect ? 1 : 0);
        InitializeQuestion();
    }
}