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
    public static DialogManager instance;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public GameObject dialogBox;
    public GameObject choiceBox;

    public float typingSpeed = 0.03f;

    public bool isActive, isTyping;

    public bool hasDialogChoice = false; //Debug
    public bool choiceMade = false; //Debug

    public TextMeshProUGUI dialogText, nameText;
    public Image dialogSelector;
    public Image choiceIcon;
    public Image charPortrait;
    public Animator animator;

    public MovingObject movingObject;

    private Queue<DialogBase.Info> dialog;
    private ChoiceController choiceController;

    public DialogChoices dialogChoices;

    public int vertical;

    // Use this for initialization
    void Start()
    {
        dialogText = dialogBox.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        nameText = dialogBox.transform.Find("Portrait").Find("Name").Find("Text").GetComponent<TextMeshProUGUI>();
        charPortrait = dialogBox.transform.Find("Portrait").Find("Image").Find("Base").Find("Portrait").GetComponent<Image>();
        dialogSelector = dialogBox.transform.Find("Selector").GetComponent<Image>();
        animator = dialogBox.GetComponent<Animator>();

        choiceBox = dialogBox.transform.Find("Choices").transform.gameObject;

        choiceController = GetComponent<ChoiceController>();

        movingObject = (MovingObject)FindObjectOfType(typeof(MovingObject));
        
        dialog = new Queue<DialogBase.Info>();
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

    public void StartDialog(DialogBase dialogBase)
    {
        dialogBox.SetActive(true);
        isActive = true;

        dialog.Clear();

        foreach (DialogBase.Info info in dialogBase.dialogInfo)
        {
            dialog.Enqueue(info);
        }

        NextSentence();
    }

    public void NextSentence()
    {
        if (dialog.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogBase.Info info = dialog.Dequeue();

        nameText.text = info.character.name;
        charPortrait.sprite = info.character.portrait;

        if (info.choices != null)
        {
            hasDialogChoice = true;
            dialogChoices = info.choices;
        }
        else
        {
            hasDialogChoice = false;
        }

        string sentence = info.sentence;

        StopAllCoroutines();
        StartCoroutine(DisplaySentence(sentence));
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
            StartCoroutine(PlaySelectorAnimation());
        
    }

    IEnumerator PlayAnimation()
    {
        animator.SetTrigger("isInActive");

        AnimatorClipInfo[] currentClip = animator.GetCurrentAnimatorClipInfo(0);
        float waitTime = currentClip[0].clip.length;

        yield return new WaitForSeconds(waitTime);

        dialogBox.SetActive(false);
    }

    IEnumerator PlaySelectorAnimation()
    {
        Animator animator = dialogSelector.GetComponent<Animator>();
        animator.SetTrigger("isInActive");

        AnimatorClipInfo[] currentClip = animator.GetCurrentAnimatorClipInfo(0);
        float waitTime = currentClip[0].clip.length;

        yield return new WaitForSeconds(waitTime);

        dialogSelector.gameObject.SetActive(false);

    }
}