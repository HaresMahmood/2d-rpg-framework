using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogManager : MonoBehaviour
{
    #region Variables
    public static DialogManager instance;

    [UnityEngine.Header("Setup")]
    public GameObject dialogContainer;

    [UnityEngine.Header("Settings")]
    [Range(0.01f, 1.0f)] [SerializeField] private float typingDelay = 0.03f;

    [HideInInspector] public bool isActive, isTyping, autoAdvance;
    [HideInInspector] public bool hasBranchingDialog = false, choiceMade = false;
    [HideInInspector] public Queue<Dialog.DialogData> dialogData;
    [HideInInspector] public BranchingDialog branchingDialog;

    private GameObject portraitContainer, autoAdvanceIcon;
    private TextMeshProUGUI dialogText;
    private Image selector;
    private Animator dialogAnimator, selectorAnimator;

    private RectTransform textTransform, dialogTransform;
    private Vector2 initTextPos, initTextDem, initDialogPos;

    private Coroutine typingCoroutine, autoAdvanceCoroutine;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        dialogAnimator = dialogContainer.GetComponent<Animator>();
        dialogText = dialogContainer.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        portraitContainer = dialogContainer.transform.Find("Portrait Container").gameObject;
        autoAdvanceIcon = dialogContainer.transform.Find("Auto Advance").gameObject;
        selector = dialogContainer.transform.Find("Selector").GetComponent<Image>();
        selectorAnimator = selector.GetComponent<Animator>();

        textTransform = dialogText.GetComponent<RectTransform>();
        initTextPos = textTransform.anchoredPosition;
        initTextDem = textTransform.sizeDelta;

        dialogTransform = dialogContainer.GetComponent<RectTransform>();
        initDialogPos = dialogTransform.anchoredPosition;

        dialogData = new Queue<Dialog.DialogData>();
    }

    private void Update()
    {
        if (autoAdvance)
            autoAdvanceCoroutine = StartCoroutine(AutoAdvance());
        else
        {
            if (autoAdvanceCoroutine != null)
                StopCoroutine(autoAdvanceCoroutine);
        }

        ToggleSelector(); // TODO: Look for more performant way to toggle selector.

        PlayerMovement player = GameManager.Player().GetComponent<PlayerMovement>();
        if (isTyping || isActive)
            player.canMove = false;
        else
            player.canMove = true;
    }
    #endregion

    public void StartDialog(Dialog dialog)
    {
        isActive = true;
        dialogContainer.SetActive(true);

        EnqueueDialog(dialog);
        NextSentence();
    }

    public void NextSentence()
    {
        if (dialogData.Count == 0)
        {
            EndDialog();
            return;
        }

        Dialog.DialogData dialog = dialogData.Dequeue();
        
        if (dialog.character != null)
        {
            TextMeshProUGUI nameText = portraitContainer.transform.Find("Name/Text").GetComponent<TextMeshProUGUI>();
            Image charPortrait = portraitContainer.transform.Find("Image").GetComponent<Image>();

            nameText.text = dialog.character.name;
            charPortrait.sprite = dialog.character.portrait;
            portraitContainer.SetActive(true);

            textTransform.sizeDelta = initTextDem;
            textTransform.anchoredPosition = initTextPos;

            dialogTransform.anchoredPosition = initDialogPos;
        }
        else
        {
            portraitContainer.SetActive(false);
            textTransform.sizeDelta = new Vector2(dialogContainer.transform.Find("Base").GetComponent<RectTransform>().rect.width - 500, initTextDem.y);
            textTransform.anchoredPosition = new Vector2(0, initTextPos.y);
            dialogTransform.anchoredPosition = new Vector2(0, initDialogPos.y);
        }

        if (dialog.branchingDialog != null)
        {
            branchingDialog = dialog.branchingDialog;
            hasBranchingDialog = true;
        }
        else
            hasBranchingDialog = false;

        string sentence = dialog.sentence;
        StopAllCoroutines();
        typingCoroutine = StartCoroutine(DisplaySentence(sentence));
    }

    IEnumerator DisplaySentence(string sentence)
    {
        isTyping = true;

        dialogText.SetText(sentence);
        dialogText.ForceMeshUpdate();

        int totalChars = dialogText.textInfo.characterCount, visibleChars = 0, counter = 0;

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
            StartCoroutine(BranchingDialogManager.instance.CreateChoiceButtons());
        }
    }

    public void EnqueueDialog(Dialog dialog)
    {
        dialogData.Clear();

        foreach (Dialog.DialogData info in dialog.dialogData)
            dialogData.Enqueue(info);
    }

    public IEnumerator AutoAdvance()
    {
        while (isTyping)
            yield return null;

        if (!isTyping && !hasBranchingDialog && autoAdvance)
        {
            yield return new WaitForSeconds(1f); // TODO: make waiting time between sentences serializable.
            NextSentence();
        }
    }

    public void EndDialog()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            isTyping = false;
        }

        isActive = false;
        StartCoroutine(PlayAnimation(dialogAnimator));
    }

    private void ToggleSelector()
    {
        if (!autoAdvance)
        {
            autoAdvanceIcon.SetActive(false);

            if (!isTyping && !hasBranchingDialog && isActive)
                selector.gameObject.SetActive(true);
            else if (!isActive)
                selector.gameObject.SetActive(false);
            else
                StartCoroutine(PlayAnimation(selectorAnimator));
        }
        else
        {
            selector.gameObject.SetActive(false);
            autoAdvanceIcon.SetActive(true);
        }
    }

    private IEnumerator PlayAnimation(Animator animator)
    {
        animator.SetTrigger("isInActive");

        float waitTime = animator.GetAnimationTime();
        yield return new WaitForSeconds(waitTime);

        animator.gameObject.SetActive(false);
    }
}