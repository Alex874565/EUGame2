using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [field: SerializeField] public DialogueUI DialogueUI { get; private set; }
    [SerializeField] private DialoguesDatabase dialoguesDatabase;

    public void TryShowDialogue()
    {
        bool shouldShowDialogue = ServiceLocator.Instance.GameManager.WonLastWave || ServiceLocator.Instance.GameManager.WaveIndex == 27;
        if (!shouldShowDialogue)
            return;
     
        int index = ServiceLocator.Instance.GameManager.WaveIndex;
        
        var dialogue = dialoguesDatabase.Dialogues[index];
        
        DialogueUI.StartDialogue(dialogue);
    }
    
    public void ShowDialogue(DialogueData dialogue)
    {
        DialogueUI.StartDialogue(dialogue);
    }
}