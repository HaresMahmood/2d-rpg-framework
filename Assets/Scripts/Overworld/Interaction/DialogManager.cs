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

    private float typingSpeed = 0.03f;

    [HideInInspector] public bool isActive, isTyping;

    [HideInInspector] public bool hasDialogChoice = false, choiceMade = false; //Debug

    private TextMeshProUGUI dialogText, nameText;
    private Image dialogSelector;
    private Image choiceIcon;
    private Image charPortrait;
    private Animator animator;

    private MovingObject movingObject;

    [HideInInspector] public Queue<Dialog.Info> dialogInfo;
    private ChoiceController choiceController;

    [HideInInspector] public DialogChoices dialogChoices;

    // Use this for initialization
    void Start()
    {
        dialogText = dialogBox.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        nameText = dialogBox.transform.Find("Portrait").Find("Name").Find("Text").GetComponent<TextMeshProUGUI>();
        charPortrait = dialogBox.transform.Find("Portrait").Find("Image").Find("Base").Find("Portrait").GetComponent<Image>();
        dialogSelector = dialogBox.transform.Find("Selector").GetComponent<Image>();
        animator = dialogBox.GetComponent<Animator>();

        

        choiceController = GetComponent<ChoiceController>();

        movingObject = (MovingObject)FindObjectOfType(typeof(MovingObject));
        
        dialogInfo = new Queue<Dialog.Info>();
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

        EnqueueDialog(dialog);
        NextSentence();
    }

    public void NextSentence()
    {
        if (dialogInfo.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialog.Info info = dialogInfo.Dequeue();

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

    public void EnqueueDialog(Dialog dialog)
    {
        dialogInfo.Clear();

        foreach (Dialog.Info info in dialog.dialogInfo)
        {
            dialogInfo.Enqueue(info);
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