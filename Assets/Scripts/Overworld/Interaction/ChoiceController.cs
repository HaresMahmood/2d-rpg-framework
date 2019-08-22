using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceController : MonoBehaviour
{
    public GameObject[] choiceButtons;
    public GameObject choiceButtonPrefab;
    public GameObject choiceObject;
    private DialogManager dialogManager;
    public EventSystem eventSystem;

    public int selected;

    public int index;
    [SerializeField] bool keyDown;
    private int maxIndex;


    // Start is called before the first frame update
    void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
        choiceObject = dialogManager.dialogBox.transform.Find("Choices").gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (index < maxIndex)
                        index++;
                    else
                        index = 0;
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (index > 0)
                        index--;
                    else
                        index = maxIndex;
                }
                keyDown = true;
            }
        }
        else
            keyDown = false;

        if (selected >= 0)
        {
            if (Input.GetButtonDown("Interact") && dialogManager.isActive && !dialogManager.isTyping && !dialogManager.choiceMade)
            {
                ClickAction();

                GameObject selector = choiceObject.transform.Find("Selector").gameObject;
                selector.SetActive(false);
                dialogManager.choiceMade = true;

                dialogManager.NextSentence();
            }
        }

        if (choiceButtons != null)
        {
            GameObject selector = choiceObject.transform.Find("Selector").gameObject;
            Vector2 choiceButtonPos = choiceButtons[selected].transform.position;

            selector.transform.position = choiceButtonPos;
            selector.SetActive(true);

            eventSystem.SetSelectedGameObject(null); //Resetting the currently selected GO
            eventSystem.firstSelectedGameObject = choiceButtons[0].transform.gameObject;
            eventSystem.SetSelectedGameObject(choiceButtons[selected].transform.gameObject);
        }
        else
            return;
    }


    public void CreateChoiceButtons()
    {
        choiceButtons = new GameObject[dialogManager.dialogChoices.choices.Length];

        int i = 0;

        for (i = 0; i < dialogManager.dialogChoices.choices.Length; i++)
        {
            GameObject choiceButtonObj = (GameObject)Instantiate(choiceButtonPrefab, Vector3.zero, Quaternion.identity);
            choiceButtonObj.name = "Choice Button: " + (i + 1);

            choiceButtonObj.transform.SetParent(choiceObject.transform.Find("Buttons").transform, false);

            choiceButtonObj.GetComponentInChildren<TextMeshProUGUI>().text = dialogManager.dialogChoices.choices[i].choiceText;

            Button choiceButton = choiceButtonObj.GetComponent<Button>();

            Vector2 pos = Vector2.zero;

            choiceButtonObj.GetComponent<RectTransform>().anchoredPosition = pos;

            choiceButtonObj.GetComponent<ButtonHandler>().buttonIndex = i;

            UnityEventHandler eventHandler = choiceButtonObj.GetComponent<UnityEventHandler>();

            eventHandler.eventHandler = dialogManager.dialogChoices.choices[i].choiceEvent;

            if (dialogManager.dialogChoices.choices[i].nextDialog != null)
                eventHandler.dialog = dialogManager.dialogChoices.choices[i].nextDialog;
            else
                eventHandler.dialog = null;

            choiceButtons[i] = choiceButtonObj;
        }

        maxIndex = choiceButtons.Length - 1;
    }

    void ClickAction()
    {
        //button clicked
        UnityEventHandler eventHandler = choiceButtons[selected].GetComponent<Button>().GetComponent<UnityEventHandler>();
        eventHandler.eventHandler.Invoke();

        if (eventHandler.dialog != null)
        {
            foreach (Dialog.Info dialogInfo in eventHandler.dialog.dialogInfo)
            dialogManager.dialogInfo.Enqueue(dialogInfo);
        }

        //destroy current buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Destroy(choiceButtons[i]);
        }

        //prepare all variable for next choice batch
        choiceButtons = null;
    }
}
