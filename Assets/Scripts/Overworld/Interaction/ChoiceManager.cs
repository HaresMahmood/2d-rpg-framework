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

    private GameObject[] choiceButtons;
    [HideInInspector] public GameObject choiceHolder, selector;

    private bool isInteracting;
    [HideInInspector] public int buttonIndex, selectedButton;
    private int maxButtonIndex;

    // Start is called before the first frame update
    private void Start()
    {
        choiceHolder = DialogManager.instance.dialogBox.transform.Find("Choices").gameObject;
        selector = choiceHolder.transform.Find("Selector").gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();

        if (selectedButton >= 0)
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
            choiceButtonObj.GetComponent<ButtonHandler>().buttonIndex = i;

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

        float timeToFade = 0.05f; // Make serializable

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            StartCoroutine(FadeButton(choiceButtons[i], 1f, timeToFade));
            yield return new WaitForSeconds(timeToFade);
        }
    }

    public IEnumerator ChoiceMade()
    {
        // Play animations.
        if (selector.gameObject.activeSelf)
        {
            Animator selectorAnimator = selector.GetComponent<Animator>();
            selectorAnimator.SetTrigger("isInActive");

            float waitTime = GetAnimationInfo(selectorAnimator);
            yield return new WaitForSeconds(waitTime / 2);
        }

        Animator choiceAnimator = choiceHolder.GetComponent<Animator>();
        choiceAnimator.SetTrigger("isInActive");

        float choiceWaitTime = GetAnimationInfo(choiceAnimator);

        float timeToFade = 0.07f; // Make serializable

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            StartCoroutine(FadeButton(choiceButtons[i], 0f, timeToFade));
            yield return new WaitForSeconds(timeToFade);
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

    private void CheckInput()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (!isInteracting)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (buttonIndex < maxButtonIndex)
                    {
                        buttonIndex++;
                    }
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

    // Define an enumerator to perform our fading.
    // Pass it the material to fade, the opacity to fade to (0 = transparent, 1 = opaque),
    // and the number of seconds to fade over.
    IEnumerator FadeButton(GameObject button, float targetOpacity, float duration)
    {

        // Cache the current color of the material, and its initiql opacity.
        Color buttonColor = button.GetComponent<Image>().color;
        Color textColor = button.GetComponentInChildren<TextMeshProUGUI>().color;
        float startOpacity = buttonColor.a;

        // Track how many seconds we've been fading.
        float t = 0;

        while (t < duration)
        {
            // Step the fade forward one frame.
            t += Time.deltaTime;
            // Turn the time into an interpolation factor between 0 and 1.
            float blend = Mathf.Clamp01(t / duration);

            // Blend to the corresponding opacity between start & target.
            buttonColor.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            // Apply the resulting color to the button.
            button.GetComponent<Image>().color = buttonColor;

            // Blend to the corresponding opacity between start & target.
            textColor.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            // Apply the resulting color to the text.
            button.GetComponentInChildren<TextMeshProUGUI>().color = buttonColor;

            // Wait one frame, and repeat.
            yield return null;
        }
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
