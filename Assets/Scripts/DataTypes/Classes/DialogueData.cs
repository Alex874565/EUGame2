using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueData
{
    [field: SerializeField] public string CharacterName { get; private set; }
    [field: SerializeField] public Sprite CharacterPortrait { get; private set; }
    [field: SerializeField] public List<string> DialogueLines { get; private set; }
}