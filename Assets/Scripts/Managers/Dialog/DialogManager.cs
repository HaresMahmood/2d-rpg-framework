using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    [HideInInspector] public bool isActive, isTyping, hasBranchingDialog = false, choiceMade = false;
    [HideInInspector] public Queue<Dialog.Info> dialogInfo;
    [HideInInspector] public DialogChoices dialogChoices;

    private GameObject charHolder;
    private TextMeshProUGUI dialogText;
    private Image dialogSelector;
    private Animator dialogAnimator, selectorAnimator;

    private RectTransform textTransform, dialogTransform;
    private Vector2 initTextPos, initTextDem, initDialogPos;

    private Coroutine typingRoutine;

    // Use this for initialization
    private void Start()
    {
        dialogText = dialogBox.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        dialogSelector = dialogBox.transform.Find("Selector").GetComponent<Image>();
        dialogAnimator = dialogBox.GetComponent<Animator>();
        selectorAnimator = dialogSelector.GetComponent<Animator>();

        textTransform = dialogText.GetComponent<RectTransform>();
        initTextPos = textTransform.anchoredPosition;
        initTextDem = textTransform.sizeDelta;

        dialogTransform = dialogBox.GetComponent<RectTransform>();
        initDialogPos = dialogTransform.anchoredPosition;

        dialogInfo = new Queue<Dialog.Info>();
    }

    private void Update()
    {
        ToggleSelector();

        PlayerMovement player = GameManager.Player().GetComponent<PlayerMovement>();
        if (isTyping || isActive)
            player.canMove = false;
        else
            player.canMove = true;
    }

    public void StartDialog(Dialog dialog)
    {
        isActive = true;
        dialogBox.SetActive(true);

        EnqueueDialog(dialog);
        NextSentence();
    }

    public void NextSentence()
    {
        if (dialogInfo.Count == 0)
        {
            EndDialog();
            return;
        }

        Dialog.Info info = dialogInfo.Dequeue();

        charHolder = dialogBox.transform.Find("Portrait").gameObject;
        if (info.character != null)
        {
            TextMeshProUGUI nameText = charHolder.transform.Find("Name").Find("Text").GetComponent<TextMeshProUGUI>();
            Image charPortrait = charHolder.transform.Find("Image").GetComponent<Image>();

            nameText.text = info.character.name;
            charPortrait.sprite = info.character.portrait;
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

        if (info.choices != null)
        {
            hasBranchingDialog = true;
            dialogChoices = info.choices;
        }
        else
            hasBranchingDialog = false;

        string sentence = info.sentence;
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
        dialogInfo.Clear();

        foreach (Dialog.Info info in dialog.dialogInfo)
            dialogInfo.Enqueue(info);
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
        if (!isTyping && !hasBranchingDialog && isActive)
            dialogSelector.gameObject.SetActive(true);
        else
            StartCoroutine(PlayAnimation(selectorAnimator));
    }

    private IEnumerator PlayAnimation(Animator animator)
    {
        animator.SetTrigger("isInActive");

        float waitTime = animator.GetAnimationTime();
        yield return new WaitForSeconds(waitTime);

        animator.gameObject.SetActive(false);
    }
}