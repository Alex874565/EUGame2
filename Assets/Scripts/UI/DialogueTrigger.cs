using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueData dialogueData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogueUI.StartDialogue(dialogueData);
        }
    }
}