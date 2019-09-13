using UnityEngine;

public class CharInteraction : InteractableObject
{
    [UnityEngine.Header("Setup")]
    public Dialog dialog;

    private CharMovement movement;

    private void Start()
    {
        movement = GetComponent<CharMovement>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (CanInteract(!DialogManager.instance.isTyping))
            {
                movement.direction = GameManager.Player().GetComponent<PlayerMovement>().orientation * -1;

                if (!DialogManager.instance.isActive)
                    StartDialogue();
                else if (DialogManager.instance.isActive && !DialogManager.instance.hasBranchingDialog)
                    NextSentence();
            }
        }

        if (Input.GetButtonDown("Cancel") && DialogManager.instance.isActive)
        {
            SkipDialog();
        }
    }

    private void StartDialogue()
    {
        DialogManager.instance.StartDialog(dialog);
    }

    private void NextSentence()
    {
        DialogManager.instance.NextSentence();
    }
}
