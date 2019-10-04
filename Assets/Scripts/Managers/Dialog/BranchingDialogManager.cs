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

    [UnityEngine.Header("Settings")]
    [SerializeField] private GameObject optionButtonPrefab;
    [Range(0.01f, 0.2f)] [SerializeField] private float buttonAnimationDelay = 0.07f;

    private GameObject[] choiceButtons;
    [HideInInspector] public GameObject optionContainer, selector;

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
        selector = optionContainer.transform.Find("Selector").gameObject;

        selectorAnim = selector.GetComponent<Animator>();
        optionContainerAnim = optionContainer.GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        CheckForInput();

        if (choiceButtons != null)
        {
            if (CanChoose())
                StartCoroutine(ChoiceMade());
        }

        if (choiceButtons != null && !DialogManager.instance.choiceMade)
        {
            Vector2 choiceButtonPos = choiceButtons[selectedButton].transform.position;

            selector.transform.position = choiceButtonPos;
            selector.SetActive(true);

            eventSystem.SetSelectedGameObject(choiceButtons[selectedButton].transform.gameObject);
        }
        else
            return;
    }

    public IEnumerator CreateChoiceButtons()
    {
        choiceButtons = new GameObject[DialogManager.instance.branchingDialog.dialogBranches.Count];

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

            choiceButtons[i] = choiceButtonObj;
        }

        maxButtonIndex = choiceButtons.Length - 1;
        optionContainer.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            StartCoroutine((choiceButtons[i].FadeObject(1f, buttonAnimationDelay)));
            yield return new WaitForSeconds(buttonAnimationDelay);
        }

        selectedButton = 0;

        eventSystem.SetSelectedGameObject(null); //Resetting the currently selected GO
        eventSystem.firstSelectedGameObject = choiceButtons[0].transform.gameObject;
    }

    public IEnumerator ChoiceMade()
    {
        destroyingButtons = true;
        // Play animations.
        if (selector.gameObject.activeSelf)
        {
            selectorAnim.SetTrigger("isInActive");

            float waitTime = selectorAnim.GetAnimationTime();
            yield return new WaitForSeconds(waitTime / 2);
        }

        optionContainerAnim.SetTrigger("isInActive");

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (choiceButtons[i] != null)
            StartCoroutine(choiceButtons[i].FadeObject(0f, buttonAnimationDelay));
            yield return new WaitForSeconds(buttonAnimationDelay);
        }

        selector.SetActive(false);
        optionContainer.SetActive(false);

        UnityEventHandler choiceEvent = choiceButtons[selectedButton].GetComponent<Button>().GetComponent<UnityEventHandler>();
        choiceEvent.eventHandler.Invoke();

        if (choiceEvent.dialog != null)
        {
            foreach (Dialog.DialogData dialog in choiceEvent.dialog.dialogData)
                DialogManager.instance.dialogData.Enqueue(dialog);
        }

        DestroyButtons();

        DialogManager.instance.choiceMade = true;
        DialogManager.instance.NextSentence();
    }

    public void DestroyButtons()
    {
        if (choiceButtons != null)
        {
            for (int i = 0; i < choiceButtons.Length; i++) // Destroy currenty displayed choice buttons.
                Destroy(choiceButtons[i]);
        }

        choiceButtons = null; // Reset choiceButtons-array to prepare for next batch of choices.
        selectedButton = 0; buttonIndex = 0;
        destroyingButtons = false;
    }

    public void SkipChoice()
    {
        selector.SetActive(true);
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
