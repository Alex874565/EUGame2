using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizzesDatabase", menuName = "ScriptableObjects/Databases/QuizzesDatabase")]
public class QuizzesDatabase : ScriptableObject
{
    [field: SerializeField] public List<QuizData> Quizzes { get; private set; }
}