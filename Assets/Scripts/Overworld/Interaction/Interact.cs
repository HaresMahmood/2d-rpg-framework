//TODO Clean up code, document and comment, fix animation bugs.
//TODO Make Interactable-class to store info for all interactables.
//TODO Move all typing functionality to seperate class?

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interact : MonoBehaviour
{

    // public string dialog;
    public static bool playerInRange;

    public string playerTag = "Player";

    public GameObject dialogBox;
    public GameObject continueIcon;
    public GameObject stopIcon;

    public TextMeshProUGUI textDisplay;
    
    public string[] sentences;

    public float typingSpeed = 0.05f;
    public float speedMultiplier = 0.5f;

    private bool _isStringBeingRevealed = false;
    private bool _isDialoguePlaying = false;
    private bool _isEndOfDialogue = false;

    void Start()
    {
        textDisplay.text = "";

        HideIcons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && playerInRange) 
        {
            if (dialogBox.activeInHierarchy && !_isDialoguePlaying)
            {
                dialogBox.SetActive(false);
            }
            else
            {
                dialogBox.SetActive(true);
                if (!_isDialoguePlaying)
                {
                    _isDialoguePlaying = true;
                    StartCoroutine(StartDialogue());
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag)) 
        {
            playerInRange = true;

            //Debug.Log("In range");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;

            //Debug.Log("Not in range");
        }
    }

    private IEnumerator StartDialogue()
    {
        int dialogueLength = sentences.Length;
        int currentDialogueIndex = 0;

        while (currentDialogueIndex < dialogueLength || !_isStringBeingRevealed)
        {
            if (!_isStringBeingRevealed)
            {
                _isStringBeingRevealed = true;
                StartCoroutine(DisplayText(sentences[currentDialogueIndex++]));

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

    IEnumerator DisplayText(string text)
    {
        int textLength = text.Length;
        int currentChar = 0;

        HideIcons();

        textDisplay.text = "";

        while (currentChar < textLength)
        {
            textDisplay.text += text[currentChar];
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
