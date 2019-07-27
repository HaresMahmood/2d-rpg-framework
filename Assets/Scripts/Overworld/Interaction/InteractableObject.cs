//TODO Clean up code, document and comment, fix animation bugs.
//TODO Stop-icon NOT appearing. (it only appears if I do NOT hold down Interact-button when the last sentece is being typed).
//TODO Move all UI-changing functionality in update function to DialogManager.
//TODO Fix bug where NPC doesn't interact and only changes orientation the first time Interact button is pressed and only interacts on second press.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public Dialog dialog;
    private DialogManager dialogManager;

    public static bool playerInRange;
    public string playerTag = "Player";
    
    void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && InteractableObject.playerInRange)
        {
            Debug.Log("INTERACTING");
            if (dialogManager.dialogBox.activeInHierarchy && !dialogManager.isActive)
                dialogManager.dialogBox.SetActive(false);
            else
            {
                if (!dialogManager.isActive)
                {
                    dialogManager.isActive = true;
                    dialogManager.dialogBox.SetActive(true);

                    TriggerDialog();
                }
            }
        }
    }

    public void TriggerDialog()
    {
       StartCoroutine(dialogManager.StartDialog(dialog.sentences));
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
