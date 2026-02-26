using UnityEngine;

[System.Serializable]
public class QuizAnswerData
{
    [field: SerializeField] public string Text { get; private set; }
    [field: SerializeField] public bool IsCorrect { get; private set; }
}