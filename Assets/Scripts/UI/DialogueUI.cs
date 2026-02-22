using UnityEngine;
using UnityEngine.UI;
using TMPro; // If you're using TextMeshPro
using System.Collections;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private DialogueData currentDialogue;
    private int currentLineIndex = 0;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool isDialogueActive = false;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (!isDialogueActive)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        portraitImage.sprite = dialogue.CharacterPortrait;
        nameText.text = dialogue.CharacterName;

        Show();
        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(currentDialogue.DialogueLines[currentLineIndex]));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void HandleClick()
    {
        // If typing → instantly finish line
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue.DialogueLines[currentLineIndex];
            isTyping = false;
        }
        else
        {
            // Go to next line
            currentLineIndex++;

            if (currentLineIndex >= currentDialogue.DialogueLines.Count)
            {
                EndDialogue();
            }
            else
            {
                DisplayCurrentLine();
            }
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}