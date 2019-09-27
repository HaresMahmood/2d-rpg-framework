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
    public GameObject dialogBox;

    [UnityEngine.Header("Settings")]
    [Range(0.01f, 1.0f)] [SerializeField] private float typingDelay = 0.03f;

    [HideInInspector] public bool isActive, isTyping, autoAdvance, hasBranchingDialog = false, choiceMade = false;
    [HideInInspector] public Queue<Dialog.DialogData> dialogData;
    [HideInInspector] public BranchingDialog branchingDialog;

    private GameObject charHolder, autoAdvanceIcon;
    private TextMeshProUGUI dialogText;
    private Image dialogSelector;
    private Animator dialogAnimator, selectorAnimator;

    private RectTransform textTransform, dialogTransform;
    private Vector2 initTextPos, initTextDem, initDialogPos;

    private Coroutine typingRoutine;
    private Coroutine autoAdvanceRoutine; // TODO: Make dictionary to keep track of all purgable Coroutines.
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
        dialogText = dialogBox.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        dialogAnimator = dialogBox.GetComponent<Animator>();
        autoAdvanceIcon = dialogBox.transform.Find("Auto advance").gameObject;
        dialogSelector = dialogBox.transform.Find("Selector").GetComponent<Image>();
        selectorAnimator = dialogSelector.GetComponent<Animator>();

        textTransform = dialogText.GetComponent<RectTransform>();
        initTextPos = textTransform.anchoredPosition;
        initTextDem = textTransform.sizeDelta;

        dialogTransform = dialogBox.GetComponent<RectTransform>();
        initDialogPos = dialogTransform.anchoredPosition;

        dialogData = new Queue<Dialog.DialogData>();

    }

    private void Update()
    {
        if (autoAdvance)
            autoAdvanceRoutine = StartCoroutine(AutoAdvance());
        else
        {
            if (autoAdvanceRoutine != null)
                StopCoroutine(autoAdvanceRoutine);
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
        dialogBox.SetActive(true);

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

        charHolder = dialogBox.transform.Find("Portrait").gameObject;
        if (dialog.character != null)
        {
            TextMeshProUGUI nameText = charHolder.transform.Find("Name").Find("Text").GetComponent<TextMeshProUGUI>();
            Image charPortrait = charHolder.transform.Find("Image").GetComponent<Image>();

            nameText.text = dialog.character.name;
            charPortrait.sprite = dialog.character.portrait;
            charHolder.SetActive(true);

            textTransform.sizeDelta = initTextDem;
            textTransform.anchoredPosition = initTextPos;

            dialogTransform.anchoredPosition = initDialogPos;
        }
        else
        {
            charHolder.SetActive(false);
            textTransform.sizeDelta = new Vector2(dialogBox.transform.Find("Base").GetComponent<RectTransform>().rect.width - 500, initTextDem.y);
            textTransform.anchoredPosition = new Vector2(0, initTextPos.y);
            dialogTransform.anchoredPosition = new Vector2(0, initDialogPos.y);
        }

        if (dialog.branchingDialog != null)
        {
            branchingDialog = dialog.branchingDialog;
            hasBranchingDialog = true;
        }
        else
        {
            hasBranchingDialog = false;
        }

        string sentence = dialog.sentence;
        StopAllCoroutines();
        typingRoutine = StartCoroutine(DisplaySentence(sentence));
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
            StartCoroutine(ChoiceManager.instance.CreateChoiceButtons());
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
            StopCoroutine(typingRoutine);
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
                dialogSelector.gameObject.SetActive(true);
            else if (!isActive)
                dialogSelector.gameObject.SetActive(false);
            else
                StartCoroutine(PlayAnimation(selectorAnimator));
        }
        else
        {
            dialogSelector.gameObject.SetActive(false);
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