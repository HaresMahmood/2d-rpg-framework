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
    public Language language = Language.English;
    [Range(0.01f, 0.09f)] [SerializeField] private float typingDelay = 0.03f;

    [HideInInspector] public bool isActive, isTyping, autoAdvance;
    [HideInInspector] public bool hasBranchingDialog = false, choiceMade = false;
    [HideInInspector] public Queue<Dialog.DialogData> dialogData;
    [HideInInspector] public BranchingDialog branchingDialog;

    private GameObject portraitContainer, skipContainer, autoAdvanceIcon;
    private TextMeshProUGUI dialogText;
    private Image selector;
    private Animator dialogAnimator, selectorAnimator;

    private float typingMultiplier = 1;

    private RectTransform textTransform, dialogTransform;
    private Vector2 initTextPos, initTextDem, initDialogPos;

    private Coroutine typingCoroutine, autoAdvanceCoroutine;

    private List<Dialog.DialogData> languageData;

    public enum Language
    {
        English,
        Dutch,
        German
    }

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
        skipContainer = dialogContainer.transform.Find("Skip Container").gameObject;
        autoAdvanceIcon = dialogContainer.transform.Find("Auto Advance").gameObject;
        selector = dialogContainer.transform.Find("Selector").GetComponent<Image>();
        selectorAnimator = selector.GetComponent<Animator>();

        textTransform = dialogText.GetComponent<RectTransform>();
        initTextPos = textTransform.anchoredPosition;
        initTextDem = textTransform.sizeDelta;

        dialogTransform = dialogContainer.GetComponent<RectTransform>();
        initDialogPos = dialogTransform.anchoredPosition;

        dialogData = new Queue<Dialog.DialogData>();
        languageData = new List<Dialog.DialogData>();
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
        switch (language)
        {
            case Language.English:
                {
                    languageData = dialog.dialogData;
                    break;
                }
            case Language.Dutch:
                {
                    languageData = dialog.dialogDataDutch;
                    break;
                }
            case Language.German:
                {
                    languageData = dialog.dialogDataGerman;
                    break;
                }
            default: break;
        }

        isActive = true;
        dialogContainer.SetActive(true);

        EnqueueDialog(languageData);
        NextSentence("Interact");
    }

    public void NextSentence(string input)
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

        if (input.Equals("Interact"))
            typingCoroutine = StartCoroutine(TypeSentence(sentence));
        else if (input.Equals("Cancel"))
            DisplaySentence(sentence);
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        if (sentence.ToLower().Contains("[player]"))
            sentence = sentence.Replace("[player]", GameManager.instance.playerName);

        dialogText.SetText(sentence);
        dialogText.ForceMeshUpdate();

        int totalChars = dialogText.textInfo.characterCount, visibleChars = 0, counter = 0;

        while (visibleChars < totalChars)
        {
            visibleChars = counter % (totalChars + 1);
            dialogText.maxVisibleCharacters = visibleChars;
            counter++;

            if (dialogText.textInfo.characterInfo[dialogText.maxVisibleCharacters].character == ' ')
                typingMultiplier = 0;
            else if (dialogText.textInfo.characterInfo[dialogText.maxVisibleCharacters].character == '.')
                typingMultiplier = 5;
            else
                typingMultiplier = 1;

            yield return new WaitForSeconds(typingDelay * typingMultiplier);

            if ((Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel")) && isTyping)
            {
                StopCoroutine(typingCoroutine);
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

    private void DisplaySentence(string sentence)
    {
        isTyping = true;

        if (sentence.ToLower().Contains("[player]"))
            sentence = sentence.Replace("[player]", GameManager.instance.playerName);

        dialogText.SetText(sentence);
        dialogText.ForceMeshUpdate();

        isTyping = false;

        if (hasBranchingDialog)
        {
            choiceMade = false;
            StartCoroutine(BranchingDialogManager.instance.CreateChoiceButtons());
        }
    }

    public void EnqueueDialog(List<Dialog.DialogData> dialog)
    {
        dialogData.Clear();

        foreach (Dialog.DialogData info in dialog)
            dialogData.Enqueue(info);
    }

    public IEnumerator AutoAdvance()
    {
        while (isTyping)
            yield return null;

        if (!isTyping && !hasBranchingDialog && autoAdvance)
        {
            yield return new WaitForSeconds(1f); // TODO: make waiting time between sentences serializable.
            NextSentence("Interact");
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

    public IEnumerator SkipDialog()
    {
        skipContainer.SetActive(true);
        Animator skipAnim = skipContainer.GetComponent<Animator>();

        float waitTime = 2f, counter = 0;

        while (counter < waitTime)
        {
            //Debug.Log(counter);
            yield return null;
            if (Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel"))
            {
                counter = waitTime;
                StartCoroutine(PlayAnimation(skipAnim));
                yield break;
            }

            else if (Input.GetButtonDown("Start") && isActive)
            {
                if (isTyping)
                {
                    StopCoroutine(typingCoroutine);
                    isTyping = false;
                }

                counter = waitTime;
                StartCoroutine(PlayAnimation(skipAnim));

                if (DialogManager.instance.hasBranchingDialog)
                    BranchingDialogManager.instance.SkipChoice();

                EndDialog();
            }

            counter += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(PlayAnimation(skipAnim));
        yield break;
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