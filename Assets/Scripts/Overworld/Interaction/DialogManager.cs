using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject continueIcon;
    public GameObject stopIcon;

    public TextMeshProUGUI textDisplay;

    public float typingSpeed = 0.05f;
    public float speedMultiplier = 0.01f;

    public bool _isStringBeingRevealed = false;
    public bool _isDialoguePlaying = false;
    public bool _isEndOfDialogue = false;

    void Start()
    {
        textDisplay.text = "";

        HideIcons();
    }

    void Update()
    {
        if (!_isStringBeingRevealed && !_isDialoguePlaying)
            dialogBox.SetActive(false);
    }

    public IEnumerator StartDialog(string[] sentences)
    {
        int dialogueLength = sentences.Length;
        int currentDialogueIndex = 0;

        while (currentDialogueIndex < dialogueLength || !_isStringBeingRevealed)
        {
            if (!_isStringBeingRevealed)
            {
                _isStringBeingRevealed = true;
                StartCoroutine(TypeSentence(sentences[currentDialogueIndex++]));

                if (currentDialogueIndex >= dialogueLength)
                {
                    _isEndOfDialogue = true;
                }
            }
            yield return 0;
        }

        while (true)
        {
            if (Input.GetButtonDown("Interact"))
                break;

            yield return 0;
        }

        HideIcons();

        _isEndOfDialogue = false;
        _isDialoguePlaying = false;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        int textLength = sentence.Length;
        int currentChar = 0;

        HideIcons();

        textDisplay.text = "";

        while (currentChar < textLength)
        {
            textDisplay.text += sentence[currentChar];
            currentChar++;

            if (currentChar < textLength)
            {
                if (Input.GetButton("Interact"))
                    yield return new WaitForSeconds(typingSpeed * speedMultiplier);
                else
                    yield return new WaitForSeconds(typingSpeed);
            }
            else
                break;
        }

        ShowIcon();

        while (true)
        {
            if (Input.GetButtonDown("Interact"))
                break;

            yield return 0;
        }

        HideIcons();

        _isStringBeingRevealed = false;
        textDisplay.text = "";
    }

    private void HideIcons()
    {
        continueIcon.SetActive(false);
        stopIcon.SetActive(false);
    }

    private void ShowIcon()
    {
        if (_isEndOfDialogue)
        {
            stopIcon.SetActive(true);
            return;
        }

        continueIcon.SetActive(true);
    }

}
