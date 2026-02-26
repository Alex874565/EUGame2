using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialoguesDatabase", menuName = "ScriptableObjects/Databases/DialoguesDatabase")]
public class DialoguesDatabase : ScriptableObject
{
    [field: SerializeField] public List<DialogueData> Dialogues { get; private set; }
}