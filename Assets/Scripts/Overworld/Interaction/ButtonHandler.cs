using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public int buttonIndex;

    // Update is called once per frame
    void Update()
    {
        if (ChoiceManager.instance.buttonIndex == buttonIndex)
            ChoiceManager.instance.selectedButton = buttonIndex;
    }
}
