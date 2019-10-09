﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BranchingDialogManager : MonoBehaviour
{
    public static BranchingDialogManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [UnityEngine.Header("Setup")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject optionButtonPrefab;

    [UnityEngine.Header("Settings")]
    [Range(0.01f, 0.2f)] [SerializeField] private float buttonAnimationDelay = 0.07f;

    private GameObject[] optionButtons;
    [HideInInspector] public GameObject optionContainer, optionIndicator;

    private Animator selectorAnim, optionContainerAnim;

    private bool isInteracting;
    [HideInInspector] public int buttonIndex, selectedButton;
    private int maxButtonIndex;

    private bool destroyingButtons = false;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        optionContainer = DialogManager.instance.dialogContainer.transform.Find("Option Container").gameObject;
        optionIndicator = optionContainer.transform.Find("Selector").gameObject;

        selectorAnim = optionIndicator.GetComponent<Animator>();
        optionContainerAnim = optionContainer.GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        CheckForInput();

        if (optionButtons != null)
        {
            if (CanChoose())
                StartCoroutine(ChoiceMade());
        }

        if (optionButtons != null && !DialogManager.instance.choiceMade)
        {
            Vector2 choiceButtonPos = optionButtons[selectedButton].transform.position;

            optionIndicator.transform.position = choiceButtonPos;
            optionIndicator.SetActive(true);

            eventSystem.SetSelectedGameObject(optionButtons[selectedButton].transform.gameObject);
        }
        else
            return;
    }

    public IEnumerator CreateChoiceButtons()
    {
        optionButtons = new GameObject[DialogManager.instance.branchingDialog.dialogBranches.Count];

        for (int i = 0; i < DialogManager.instance.branchingDialog.dialogBranches.Count; i++)
        {
            GameObject choiceButtonObj = (GameObject)Instantiate(optionButtonPrefab, Vector3.zero, Quaternion.identity); // Instantiates/creates new choice button from prefab in scene.

            choiceButtonObj.name = "Option Button " + (i + 1); // Gives appropriate name to newly instantiated choice button.
            choiceButtonObj.transform.SetParent(optionContainer.transform.Find("Option Buttons").transform, false);
            choiceButtonObj.GetComponentInChildren<TextMeshProUGUI>().text = DialogManager.instance.branchingDialog.dialogBranches[i].branchOption;
            choiceButtonObj.GetComponent<ChoiceSelection>().buttonIndex = i;

            UnityEventHandler eventHandler = choiceButtonObj.GetComponent<UnityEventHandler>();
            eventHandler.eventHandler = DialogManager.instance.branchingDialog.dialogBranches[i].branchEvent;

            if (DialogManager.instance.branchingDialog.dialogBranches[i].nextDialog != null)
                eventHandler.dialog = DialogManager.instance.branchingDialog.dialogBranches[i].nextDialog;
            else
                eventHandler.dialog = null;

            Color color = choiceButtonObj.GetComponent<Button>().GetComponent<Image>().color;
            color.a = 0;
            choiceButtonObj.GetComponent<Button>().GetComponent<Image>().color = color;

            optionButtons[i] = choiceButtonObj;
        }

        maxButtonIndex = optionButtons.Length - 1;
        optionContainer.SetActive(true);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            StartCoroutine((optionButtons[i].FadeObject(1f, buttonAnimationDelay)));
            yield return new WaitForSeconds(buttonAnimationDelay);
        }

        selectedButton = 0;

        eventSystem.SetSelectedGameObject(null); //Resetting the currently selected GO
        eventSystem.firstSelectedGameObject = optionButtons[0].transform.gameObject;
    }

    public IEnumerator ChoiceMade()
    {
        destroyingButtons = true;
        // Play animations.
        if (optionIndicator.gameObject.activeSelf)
        {
            selectorAnim.SetTrigger("isInActive");

            float waitTime = selectorAnim.GetAnimationTime();
            yield return new WaitForSeconds(waitTime / 3);
        }

        optionContainerAnim.SetTrigger("isInActive");

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (optionButtons[i] != null)
                StartCoroutine(optionButtons[i].FadeObject(0f, buttonAnimationDelay));
            yield return new WaitForSeconds(buttonAnimationDelay);
        }

        optionIndicator.SetActive(false);
        optionContainer.SetActive(false);

        UnityEventHandler choiceEvent = optionButtons[selectedButton].GetComponent<Button>().GetComponent<UnityEventHandler>();
        choiceEvent.eventHandler.Invoke();

        if (choiceEvent.dialog != null)
        {
            foreach (Dialog.DialogData dialog in choiceEvent.dialog.dialogData)
                DialogManager.instance.dialogData.Enqueue(dialog);
        }

        DestroyButtons();

        DialogManager.instance.choiceMade = true;
        DialogManager.instance.NextSentence("Interact");
    }

    public void DestroyButtons()
    {
        if (optionButtons != null)
        {
            for (int i = 0; i < optionButtons.Length; i++) // Destroy currenty displayed choice buttons.
                Destroy(optionButtons[i]);
        }

        optionButtons = null; // Reset optionButtons-array to prepare for next batch of choices.
        selectedButton = 0; buttonIndex = 0;
        destroyingButtons = false;
    }

    public void SkipChoice()
    {
        optionIndicator.SetActive(true);
        optionContainer.SetActive(false);

        DestroyButtons();
    }

    private bool CanChoose()
    {
        if (Input.GetButtonDown("Interact") && !DialogManager.instance.isTyping && !DialogManager.instance.choiceMade && !destroyingButtons)
            return true;

        return false;
    }

    private void CheckForInput()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (!isInteracting)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (buttonIndex < maxButtonIndex)
                        buttonIndex++;
                    else
                        buttonIndex = 0;
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (buttonIndex > 0)
                        buttonIndex--;
                    else
                        buttonIndex = maxButtonIndex;
                }
                isInteracting = true;
            }
        }
        else
            isInteracting = false;
    }
}
