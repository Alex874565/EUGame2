using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class QuizData
{
    [field: SerializeField] public string Question { get; private set; }
    [field: SerializeField] public List<QuizAnswerData> Answers { get; private set; }
    
    public List<QuizAnswerData> GetRandomizedAnswers()
    {
        List<QuizAnswerData> randomizedAnswers = new List<QuizAnswerData>(Answers);
        for (int i = 0; i < randomizedAnswers.Count; i++)
        {
            QuizAnswerData temp = randomizedAnswers[i];
            int randomIndex = UnityEngine.Random.Range(i, randomizedAnswers.Count);
            randomizedAnswers[i] = randomizedAnswers[randomIndex];
            randomizedAnswers[randomIndex] = temp;
        }
        return randomizedAnswers;
    }
}