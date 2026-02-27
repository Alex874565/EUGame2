using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialoguesDatabase dialoguesDatabase;

    public void TryShowDialogue()
    {
        bool shouldShowDialogue = ServiceLocator.Instance.GameManager.WonLastWave;
        if (!shouldShowDialogue)
            return;
     
        int index = ServiceLocator.Instance.GameManager.WaveIndex;
        
        var dialogue = dialoguesDatabase.Dialogues[index];
        
        dialogueUI.StartDialogue(dialogue);
    }
}