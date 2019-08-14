//TODO: 
//- Change variable names.
//- Look into animations by watching
//Brackey's video.
//TODO: Add "[UnityEngine.Header("Configuration")]"

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject choiceBox;

    public float typingSpeed = 0.03f;

    public bool isActive, isTyping;

    public bool hasDialogChoice; //Debug
    public bool choiceMade = false; //Debug

    private TextMeshProUGUI dialogText, nameText;
    private Image continueIcon;
    public Image choiceIcon;
    private Animator animator;

    public MovingObject movingObject;

    private Queue<string> sentences;

    private Queue<DialogConfig> dialogSentences;

    public string[] dialogChoices;
    public GameObject[] choiceButtons;
    public GameObject choiceButtonPrefab;
    public float xPos;
    public float yPosOffset = 40f;

    public int vertical;

    // Use this for initialization
    void Start()
    {
        dialogText = dialogBox.transform.Find("Dialog Text").GetComponent<TextMeshProUGUI>();
        nameText = dialogBox.transform.Find("Name").Find("Name Text").GetComponent<TextMeshProUGUI>();
        continueIcon = dialogBox.transform.Find("Icons").Find("Continue").GetComponent<Image>();
        choiceIcon = choiceBox.transform.Find("Icons").Find("Select").GetComponent<Image>();
        animator = dialogBox.GetComponent<Animator>();

        movingObject = (MovingObject)FindObjectOfType(typeof(MovingObject));

        sentences = new Queue<string>();

        dialogSentences = new Queue<DialogConfig>();
    }

    void Update()
    {
        ToggleIcon(); //TODO: Should NOT toggle icon every frame!

        if (isTyping || isActive)
            movingObject.canMove = false;
        else
            movingObject.canMove = true;

        //Debug.Log(dialogSentences.Dequeue().hasChoices);
    }

    public void StartDialog(Dialog dialog)
    {
        dialogBox.SetActive(true);
        isActive = true;

        nameText.text = dialog.charName;

        dialogSentences.Clear();

        foreach (DialogConfig dialogSentence in dialog.dialog) //TODO: Change name of DialogSentences.
        {
            sentences.Enqueue(dialogSentence.sentence);

        }

        foreach (DialogConfig dialogSentence in dialog.dialog) //TODO: Change name of DialogSentences.
        {
            dialogSentences.Enqueue(dialogSentence);
        }


        NextSentence();
    }

    public void NextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (dialogSentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        DialogConfig dialogSentence = dialogSentences.Dequeue();
        hasDialogChoice = dialogSentence.hasChoices;

        if (hasDialogChoice && dialogSentence.choices.Length != 0)
        {
            dialogChoices = dialogSentence.choices;
        }


        StopAllCoroutines();
        StartCoroutine(DisplaySentence(dialogSentence.sentence));        
    }

    IEnumerator DisplaySentence(string sentence)
    {
        //TODO: VERY inefficient, do not put this section in DisplaySentence
        if (!hasDialogChoice)
            choiceBox.SetActive(false);

        isTyping = true;

        dialogText.SetText(sentence);
        dialogText.ForceMeshUpdate();

        int totalChars = dialogText.textInfo.characterCount;
        int visibleChars = 0;
        int counter = 0;

        while (visibleChars < totalChars)
        {
            visibleChars = counter % (totalChars + 1);

            dialogText.maxVisibleCharacters = visibleChars;

            counter++;
            yield return new WaitForSeconds(typingSpeed);

            if (Input.GetButtonDown("Interact") && isTyping)
            {
                visibleChars = totalChars;
                dialogText.maxVisibleCharacters = visibleChars;
                dialogText.ForceMeshUpdate();
                break;
            }
        }

        isTyping = false;

        if (hasDialogChoice)
        {
            CreateChoiceButtons();
        }
    }

    private void CreateChoiceButtons()
    {
        choiceButtons = new GameObject[dialogChoices.Length];

        int i = 0;
        float offsetCounter = 0;

        foreach (string choice in dialogChoices)
        {
            GameObject choiceButtonObj = (GameObject)Instantiate(choiceButtonPrefab, Vector3.zero, Quaternion.identity);
            choiceButtonObj.name = "ChoiceButton: " + i;

            choiceButtonObj.transform.SetParent(choiceBox.transform.Find("Option Boxes").transform, false);

            Button choiceButton = choiceButtonObj.GetComponent<Button>();

            choiceButton.GetComponentInChildren<Text>().text = dialogChoices[i];

            Vector2 pos = Vector2.zero;
            pos.y = offsetCounter;
            pos.x = xPos;

            choiceButton.GetComponent<RectTransform>().anchoredPosition = pos;

            offsetCounter -= yPosOffset;

            choiceButtons[i] = choiceButtonObj;

            i++;

        }

    }

    void EndDialogue()
    {
        isActive = false;

        if (animator.gameObject.activeSelf)
            StartCoroutine(PlayAnimation());
    }

    void ToggleIcon()
    {
        if (!isTyping && !hasDialogChoice && isActive)
            continueIcon.gameObject.SetActive(true);
        else
            continueIcon.gameObject.SetActive(false);

        if (!isTyping && hasDialogChoice && isActive)
            choiceIcon.gameObject.SetActive(true);
        else
            choiceIcon.gameObject.SetActive(false);
    }

    IEnumerator PlayAnimation()
    {
        animator.SetTrigger("isInactive");
        yield return new WaitForSeconds(0.25f);

        dialogBox.SetActive(false);
    }
}