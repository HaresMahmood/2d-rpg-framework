//TODO Clean up code, document and comment, fix animation bugs.
//TODO Stop-icon NOT appearing. (it only appears if I do NOT hold down Interact-button when the last sentece is being typed).
//TODO Move all UI-changing functionality in update function to DialogManager.
//TODO Animations COMPLETELY broken. Delete and remake UI animations!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (!dialogManager.isActive && dialogManager.hasEnded)
            {
                Debug.Log("TRUE");
                dialogManager.anim.SetBool("isOpen", true);
            }

            if (dialogManager.dialogBox.activeInHierarchy && !dialogManager.isActive)
            {
                //dialogManager.anim.SetBool("isOpen", false);
                dialogManager.dialogBox.SetActive(false);
            }
            else
            {
                if (!dialogManager.isActive)
                {
                    dialogManager.isActive = true;
                    //dialogManager.anim.SetBool("isOpen", dialogManager.hasEnded);
                    //dialogManager.anim.SetBool("isOpen", true);
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
