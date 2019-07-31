//TODO: Change variable names.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private DialogManager dialogManager;
    public Dialog dialogue;

    public static bool playerInRange;
    public string playerTag = "Player";

    void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
    }

    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetButtonDown("Interact") && !dialogManager.isActive && !dialogManager.isTyping)
            {
                TriggerDialogue();
            }
            else if (Input.GetButtonDown("Interact") && dialogManager.isActive && !dialogManager.isTyping)
            {
                dialogManager.DisplayNextSentence();
            }
        }
    }

    public void TriggerDialogue()
    {
        dialogManager.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
        }
    }

}
