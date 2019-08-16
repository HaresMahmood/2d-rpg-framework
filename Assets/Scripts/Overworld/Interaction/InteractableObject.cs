//TODO: Change variable names.

using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [UnityEngine.Header("Dialog")]
    public Dialog dialog;
    private DialogManager dialogManager;

    [UnityEngine.Header("Configuration")]
    public string playerTag = "Player";
    [HideInInspector] public static bool playerInRange;


    void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
    }

    void Update()
    {
        if (playerInRange && Input.GetButtonDown("Interact") && !dialogManager.isTyping)
        {
            if (!dialogManager.isActive)
                StartDialogue();
            else if (dialogManager.isActive)
                NextSentence();
        }
    }

    public void StartDialogue()
    {
        dialogManager.StartDialog(dialog);
    }

    public void NextSentence()
    {
        dialogManager.NextSentence();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            playerInRange = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            playerInRange = false;
    }

}