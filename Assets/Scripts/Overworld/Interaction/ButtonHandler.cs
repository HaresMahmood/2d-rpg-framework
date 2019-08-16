using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    ChoiceController choiceController;
    public int buttonIndex;

    // Start is called before the first frame update
    void Start()
    {
      choiceController = FindObjectOfType<ChoiceController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (choiceController.index == buttonIndex)
        {
            choiceController.selected = buttonIndex;
        }
    }
}
