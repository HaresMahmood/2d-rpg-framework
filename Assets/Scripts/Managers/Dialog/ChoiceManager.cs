using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [UnityEngine.Header("Setup")]
    [SerializeField] private EventSystem eventSystem;

    [UnityEngine.Header("Settings")]
    [SerializeField] private GameObject choiceButtonPrefab;
    [Range(0.01f, 0.2f)] [SerializeField] private float buttonAnimationSpeed = 0.07f;

    private GameObject[] choiceButtons;
    [HideInInspector] public GameObject choiceHolder, selector;

    private Animator selectorAnimator;
    private Animator choiceHolderAnimator;

    private bool isInteracting;
    [HideInInspector] public int buttonIndex, selectedButton;
    private int maxButtonIndex;

    // Start is called before the first frame update
    private void Start()
    {
        choiceHolder = DialogManager.instance.dialogBox.transform.Find("Choices").gameObject;
        selector = choiceHolder.transform.Find("Selector").gameObject;

        selectorAnimator = selector.GetComponent<Animator>();
        choiceHolderAnimator   = choiceHolder.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckForInput();

        if (choiceButtons != null)
        {
            if (CanMakeChoice())
                StartCoroutine(ChoiceMade());
        }

        if (choiceButtons != null && !DialogManager.instance.choiceMade)
        {
            Vector2 choiceButtonPos = choiceButtons[selectedButton].transform.position;

            selector.transform.position = choiceButtonPos;
            selector.SetActive(true);

            eventSystem.SetSelectedGameObject(null); //Resetting the currently selected GO
            eventSystem.firstSelectedGameObject = choiceButtons[0].transform.gameObject;
            eventSystem.SetSelectedGameObject(choiceButtons[selectedButton].transform.gameObject);
        }
        else
            return;
    }

    public IEnumerator CreateChoiceButtons()
    {
        choiceButtons = new GameObject[DialogManager.instance.dialogChoices.choices.Length];

        for (int i = 0; i < DialogManager.instance.dialogChoices.choices.Length; i++)
        {
            GameObject choiceButtonObj = (GameObject)Instantiate(choiceButtonPrefab, Vector3.zero, Quaternion.identity); // Instantiates (creates) new choice button from prefab in scene.

            choiceButtonObj.name = "Choice Button " + (i + 1); // Gives appropriate name to newly instantiated choice button.
            choiceButtonObj.transform.SetParent(choiceHolder.transform.Find("Buttons").transform, false);
            choiceButtonObj.GetComponentInChildren<TextMeshProUGUI>().text = DialogManager.instance.dialogChoices.choices[i].choiceText;
            choiceButtonObj.GetComponent<ChoiceSelection>().buttonIndex = i;

            UnityEventHandler eventHandler = choiceButtonObj.GetComponent<UnityEventHandler>();
            eventHandler.eventHandler = DialogManager.instance.dialogChoices.choices[i].choiceEvent;

            if (DialogManager.instance.dialogChoices.choices[i].nextDialog != null)
                eventHandler.dialog = DialogManager.instance.dialogChoices.choices[i].nextDialog;
            else
                eventHandler.dialog = null;

            Color color = choiceButtonObj.GetComponent<Button>().GetComponent<Image>().color;
            color.a = 0;
            choiceButtonObj.GetComponent<Button>().GetComponent<Image>().color = color;

            choiceButtons[i] = choiceButtonObj;
        }

        maxButtonIndex = choiceButtons.Length - 1;
        choiceHolder.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            StartCoroutine((choiceButtons[i].FadeObject(1f, buttonAnimationSpeed)));
            yield return new WaitForSeconds(buttonAnimationSpeed);
        }
    }

    public IEnumerator ChoiceMade()
    {
        // Play animations.
        if (selector.gameObject.activeSelf)
        {
            selectorAnimator.SetTrigger("isInActive");

            float waitTime = selectorAnimator.GetAnimationInfo();
            yield return new WaitForSeconds(waitTime / 2);
        }
        
        choiceHolderAnimator.SetTrigger("isInActive");

        float choiceWaitTime = choiceHolderAnimator.GetAnimationInfo();

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            StartCoroutine(choiceButtons[i].FadeObject(0f, buttonAnimationSpeed));
            yield return new WaitForSeconds(buttonAnimationSpeed);
        }

        choiceHolder.SetActive(false);

        UnityEventHandler choiceEvent = choiceButtons[selectedButton].GetComponent<Button>().GetComponent<UnityEventHandler>();
        choiceEvent.eventHandler.Invoke();

        if (choiceEvent.dialog != null)
        {
            foreach (Dialog.Info dialogInfo in choiceEvent.dialog.dialogInfo)
                DialogManager.instance.dialogInfo.Enqueue(dialogInfo);
        }

        for (int i = 0; i < choiceButtons.Length; i++) // Destroy currenty displayed choice buttons.
            Destroy(choiceButtons[i]);

        choiceButtons = null; // Reset choiceButtons-array to prepare for next batch of choices.

        DialogManager.instance.choiceMade = true;
        DialogManager.instance.NextSentence();
    }

    private bool CanMakeChoice()
    {
        if (Input.GetButtonDown("Interact") && !DialogManager.instance.isTyping && !DialogManager.instance.choiceMade)
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
