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

    private TextMeshProUGUI dialogText, nameText;
    private Image continueIcon;
    public Image choiceIcon;
    private Animator animator;

    public MovingObject movingObject;

    private Queue<string> sentences;
    private Queue<DialogSentences> dialogSentences;
    public string[] dialogChoices;

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

        dialogSentences = new Queue<DialogSentences>();

        dialogChoices = new string[3];
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

        nameText.text = dialog.name;

        dialogSentences.Clear();

        foreach (DialogSentences dialogSentence in dialog.sentences) //TODO: Change name of DialogSentences.
        {
            sentences.Enqueue(dialogSentence.sentence);

        }

        foreach (DialogSentences dialogSentence in dialog.sentences) //TODO: Change name of DialogSentences.
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

        DialogSentences dialogSentence = dialogSentences.Dequeue();
        hasDialogChoice = dialogSentence.hasChoices;

        if (hasDialogChoice && dialogSentence.choices.Length != 0)
        {
            for (int i = 0; i < dialogSentence.choices.Length; i++)
            {
                Debug.Log(dialogSentence.choices[i]);
                dialogChoices[i] = dialogSentence.choices[i];
            }
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

        DisplayChoices();
    }

    private void DisplayChoices()
    {
        if (hasDialogChoice)
            choiceBox.SetActive(true);
        else
            choiceBox.SetActive(false);

        GameObject selected = choiceBox.transform.Find("Option Boxes").Find("Option Box").gameObject;
        choiceIcon.transform.position = new Vector2(selected.transform.position.x - 350, selected.transform.position.y);

        TextMeshProUGUI text1 = choiceBox.transform.Find("Text").Find("Text (Option 1)").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI text2 = choiceBox.transform.Find("Text").Find("Text (Option 2)").GetComponent<TextMeshProUGUI>();

        text1.SetText(dialogChoices[0]);
        text2.SetText(dialogChoices[1]);

        int vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (vertical == 1)
        {
            selected = choiceBox.transform.Find("Option Boxes").Find("Option Box").gameObject;
            choiceIcon.transform.position = new Vector2(selected.transform.position.x - 350, selected.transform.position.y);
            Debug.Log(vertical);
        }
        else if (vertical == -1)
        {
            selected = choiceBox.transform.Find("Option Boxes").Find("Option Box (1)").gameObject;
            choiceIcon.transform.position = new Vector2(selected.transform.position.x - 350, selected.transform.position.y);
            Debug.Log(vertical);
        }

        if (Input.GetButtonDown("Interact"))
        {
            //Debug.Log the choice
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