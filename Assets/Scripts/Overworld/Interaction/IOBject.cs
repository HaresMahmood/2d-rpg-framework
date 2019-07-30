//TODO: Change variable names.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOBject : MonoBehaviour //TODO: Change class name to "InteractableObject".
{
    private DManager dialogManager;


    public Dialog dialogue;

    void Start()
    {
        dialogManager = FindObjectOfType<DManager>();
    }

    void Update()
    {
        //TODO: Put "!dialogManager.isTyping" in if-statements.

        if (Input.GetButtonDown("Interact") && !dialogManager.isActive)
        {
            if (!dialogManager.isTyping)
            {
                TriggerDialogue();
            }
        }
        else if (Input.GetButtonDown("Interact") && dialogManager.isActive)
        {
            if (!dialogManager.isTyping)
            {
                dialogManager.DisplayNextSentence();
            }
        }
    }

    public void TriggerDialogue()
    {
        dialogManager.StartDialogue(dialogue);
    }

}
