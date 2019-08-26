using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormManager : MonoBehaviour
{
    public static FormManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    public bool isInteracting;
    public int buttonIndex, selectedButton;
    public int maxButtonIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        selectedButton = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Selected button: " + selectedButton);
        }
    }

    private void CheckInput()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            if (!isInteracting)
            {
                if (Input.GetAxis("Horizontal") < 0)
                {
                    if (buttonIndex < maxButtonIndex)
                    {
                        buttonIndex++;
                    }
                    else
                        buttonIndex = 0;
                }
                else if (Input.GetAxis("Horizontal") > 0)
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
