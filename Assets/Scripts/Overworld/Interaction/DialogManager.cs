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

    [UnityEngine.Header("Setup")]
    public GameObject dialogBox;

    [UnityEngine.Header("Settings")]
    [Range(0.01f, 1.0f)] [SerializeField] private float typingDelay = 0.03f;

    private TextMeshProUGUI dialogText, nameText;
    private Image dialogSelector, charPortrait;
    private Animator animator;
    private MovingObject movingObject;

    [HideInInspector] public bool isActive, isTyping, hasBranchingDialog = false, choiceMade = false;

    [HideInInspector] public Queue<Dialog.Info> dialogInfo;

    [HideInInspector] public DialogChoices dialogChoices;

    // Use this for initialization
    void Start()
    {
        dialogText = dialogBox.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        nameText = dialogBox.transform.Find("Portrait").Find("Name").Find("Text").GetComponent<TextMeshProUGUI>();
        charPortrait = dialogBox.transform.Find("Portrait").Find("Image").Find("Base").Find("Portrait").GetComponent<Image>();
        dialogSelector = dialogBox.transform.Find("Selector").GetComponent<Image>();
        animator = dialogBox.GetComponent<Animator>();

        movingObject = (MovingObject)FindObjectOfType(typeof(MovingObject));
        
        dialogInfo = new Queue<Dialog.Info>();
    }

    private void Update()
    {
        ToggleSelector();

        if (isTyping || isActive)
        {
            if (movingObject.isMoving)
            {
                Debug.Log(movingObject);
                movingObject.isMoving = false;
                Debug.Log(movingObject.isMoving);
            }

            movingObject.canMove = false;
        }
        else
            movingObject.canMove = true;
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
            hasBranchingDialog = true;
            dialogChoices = info.choices;
        }
        else
            hasBranchingDialog = false;

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

            yield return new WaitForSeconds(typingDelay);

            if (Input.GetButtonDown("Interact") && isTyping)
            {
                visibleChars = totalChars;
                dialogText.maxVisibleCharacters = visibleChars;
                dialogText.ForceMeshUpdate();
                break;
            }
        }

        isTyping = false;

        if (hasBranchingDialog)
        {
            choiceMade = false;
            ChoiceManager.instance.CreateChoiceButtons();
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
        if (!isTyping && !hasBranchingDialog && isActive)
            dialogSelector.gameObject.SetActive(true);
        else
            StartCoroutine(PlaySelectorAnimation());
        
    }

    IEnumerator PlayAnimation()
    {
        animator.SetTrigger("isInActive");

        float waitTime = GetAnimationInfo(animator);
        yield return new WaitForSeconds(waitTime);

        dialogBox.SetActive(false);
    }

    IEnumerator PlaySelectorAnimation()
    {
        Animator animator = dialogSelector.GetComponent<Animator>();

        animator.SetTrigger("isInActive");

        float waitTime = GetAnimationInfo(animator);
        yield return new WaitForSeconds(waitTime);

        dialogSelector.gameObject.SetActive(false);
    }

    private float GetAnimationInfo(Animator animator)
    {
        AnimatorClipInfo[] currentClip = null;
        float waitTime = 0;

        currentClip = animator.GetCurrentAnimatorClipInfo(0);
        if (currentClip.Length > 0)
            waitTime = currentClip[0].clip.length;

        return waitTime;
    }
}