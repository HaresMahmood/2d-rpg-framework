//TODO: Change variable names.

using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [UnityEngine.Header("Setup")]
    public Dialog dialog;
    
    [HideInInspector] public static bool playerInRange;

    void Update()
    {
        if (CanInteract())
        {
            if (!DialogManager.instance.isActive)
                StartDialogue();
            else if (DialogManager.instance.isActive && !DialogManager.instance.hasBranchingDialog)
                NextSentence();
        }
        else if (Input.GetButtonDown("Back") && DialogManager.instance.isActive)
        {
            StopAllCoroutines();
            DialogManager.instance.EndDialog();
        }
    }

    public void StartDialogue()
    {
        DialogManager.instance.StartDialog(dialog);
    }

    public void NextSentence()
    {
        DialogManager.instance.NextSentence();
    }

    private bool CanInteract()
    {
        if (playerInRange && Input.GetButtonDown("Interact") && !DialogManager.instance.isTyping)
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.instance.playerTag))
            playerInRange = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.instance.playerTag))
            playerInRange = false;
    }

}