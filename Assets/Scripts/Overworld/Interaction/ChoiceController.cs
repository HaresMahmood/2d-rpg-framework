using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceController : MonoBehaviour
{
    public GameObject[] choiceButtons;
    public GameObject choiceButtonPrefab;
    public float xPos;
    public float yPosOffset = 300f;
    public DialogManager dialogManager;

    public int selected;

    public int index;
    [SerializeField] bool keyDown;
    private int maxIndex;


    // Start is called before the first frame update
    void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
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
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }

        if (selected >= 0)
        {
            if (Input.GetButtonDown("Interact") && dialogManager.isActive && !dialogManager.isTyping && !dialogManager.choiceMade)
            {
                Debug.Log("Button pressed: " + selected);

                ClickAction();

                GameObject selector = dialogManager.choiceBox.transform.Find("Selector").gameObject;
                selector.SetActive(false);
                dialogManager.choiceMade = true;

                dialogManager.NextSentence();
            }
        }

        if (choiceButtons != null)
        {
            GameObject selector = dialogManager.choiceBox.transform.Find("Selector").gameObject;
            Vector2 choiceButtonPos = choiceButtons[selected].transform.position;

            selector.transform.position = choiceButtonPos;
            selector.SetActive(true);
        }
        else
            return;
    }

    public void CreateChoiceButtons()
    {
        choiceButtons = new GameObject[dialogManager.dialogChoices.Length];

        int i = 0;
        float offsetCounter = 0;

        foreach (string choice in dialogManager.dialogChoices)
        {
            GameObject choiceButtonObj = (GameObject)Instantiate(choiceButtonPrefab, Vector3.zero, Quaternion.identity);
            choiceButtonObj.name = "ChoiceButton: " + i;

            choiceButtonObj.transform.SetParent(dialogManager.choiceBox.transform, false);

            //Button choiceButton = choiceButtonObj.GetComponent<Button>();

            choiceButtonObj.GetComponentInChildren<TextMeshProUGUI>().text = dialogManager.dialogChoices[i];

            Vector2 pos = Vector2.zero;
            pos.y = offsetCounter;
            pos.x = xPos;

            choiceButtonObj.GetComponent<RectTransform>().anchoredPosition = pos;

            offsetCounter -= yPosOffset;

            choiceButtonObj.GetComponent<ButtonHandler>().buttonIndex = i;

            choiceButtons[i] = choiceButtonObj;

            i++;
        }

        maxIndex = choiceButtons.Length;
    }

    void ClickAction()
    {
        //button clicked

        //destroy current buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Destroy(choiceButtons[i]);
        }

        //prepare all variable for next choice batch
        choiceButtons = null;
    }
}
