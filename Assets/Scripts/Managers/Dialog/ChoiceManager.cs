using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [Range(0.01f, 0.2f)] [SerializeField] private float buttonAnimationDelay = 0.07f;

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
        choiceHolderAnimator = choiceHolder.GetComponent<Animator>();
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
            GameObject choiceButtonObj = (GameObject)Instantiate(choiceButtonPrefab, Vector3.zero, Quaternion.identity); // Instantiates/creates new choice button from prefab in scene.

            choiceButtonObj.name = "Choice Button " + (i + 1); // Gives appropriate name to newly instantiated choice button.
            choiceButtonObj.transform.SetParent(choiceHolder.transform.Find("Buttons").transform, false);
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
        choiceHolder.SetActive(true);

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
        // Play animations.
        if (selector.gameObject.activeSelf)
        {
            selectorAnimator.SetTrigger("isInActive");

            float waitTime = selectorAnimator.GetAnimationTime();
            yield return new WaitForSeconds(waitTime / 2);
        }

        choiceHolderAnimator.SetTrigger("isInActive");

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            StartCoroutine(choiceButtons[i].FadeObject(0f, buttonAnimationDelay));
            yield return new WaitForSeconds(buttonAnimationDelay);
        }

        selector.SetActive(false);
        choiceHolder.SetActive(false);

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
    }

    public void SkipChoice()
    {
        selector.SetActive(true);
        choiceHolder.SetActive(false);

        DestroyButtons();
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
