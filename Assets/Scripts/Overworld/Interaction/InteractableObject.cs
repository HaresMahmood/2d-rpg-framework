//TODO Clean up code, document and comment, fix animation bugs.
//TODO Stop-icon NOT appearing. (it only appears if I do NOT hold down Interact-button when the last sentece is being typed).
//TODO Move all UI-changing functionality in update function to DialogManager.

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
        if (Input.GetButtonDown("Interact") && playerInRange)
        {
            if (dialogManager.dialogBox.activeInHierarchy && !dialogManager._isDialoguePlaying)
                dialogManager.dialogBox.SetActive(false);
            else
            {
                if (!dialogManager._isDialoguePlaying)
                {
                    dialogManager._isDialoguePlaying = true;

                    dialogManager.dialogBox.SetActive(true);
                    TriggerDialog();
                }
            }
        }
    }

    public void TriggerDialog()
    {
        if (dialogManager._isDialoguePlaying)
            StartCoroutine(dialogManager.StartDialog(dialog.sentences));
        else
            return;
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
