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

    public TextMeshProUGUI dialogText, nameText;
    public Image dialogSelector;
    public Image choiceIcon;
    public Animator animator;

    public MovingObject movingObject;

    private Queue<DialogTree> dialogTree;
    private ChoiceController choiceController;

    public string[] dialogChoices;

    public int vertical;

    // Use this for initialization
    void Start()
    {
        dialogText = dialogBox.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        nameText = dialogBox.transform.Find("Portrait").Find("Name").Find("Text").GetComponent<TextMeshProUGUI>();
        dialogSelector = dialogBox.transform.Find("Selector").GetComponent<Image>();
        animator = dialogBox.GetComponent<Animator>();

        choiceBox = dialogBox.transform.Find("Choices").transform.gameObject;

        choiceController = GetComponent<ChoiceController>();

        movingObject = (MovingObject)FindObjectOfType(typeof(MovingObject));
        
        dialogTree = new Queue<DialogTree>();
    }

    void Update()
    {
        ToggleSelector(); //TODO: Should NOT toggle icon every frame!

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

        dialogTree.Clear();

        foreach (DialogTree dialogBranch in dialog.dialogTree)
        {
            dialogTree.Enqueue(dialogBranch);
        }

        NextSentence();
    }

    public void NextSentence()
    {
        if (dialogTree.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogTree dialogBranch = dialogTree.Dequeue();
        hasDialogChoice = dialogBranch.hasChoices;

        if (hasDialogChoice && dialogBranch.choices.Length != 0)
        {
            dialogChoices = dialogBranch.choices;
        }

        StopAllCoroutines();
        StartCoroutine(DisplaySentence(dialogBranch.sentence));
    }

    IEnumerator DisplaySentence(string sentence)
    {
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
            choiceMade = false;
            choiceController.CreateChoiceButtons();
        }
    }

    void EndDialogue()
    {
        isActive = false;

        if (animator.gameObject.activeSelf)
            StartCoroutine(PlayAnimation());
    }

    void ToggleSelector()
    {
        if (!isTyping && !hasDialogChoice && isActive)
            dialogSelector.gameObject.SetActive(true);
        else
            dialogSelector.gameObject.SetActive(false);
        
    }

    IEnumerator PlayAnimation()
    {
        animator.SetTrigger("isInActive");
        yield return new WaitForSeconds(0.20f);

        dialogBox.SetActive(false);
    }
}