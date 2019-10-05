using UnityEngine;

public class CharInteraction : InteractableObject
{
    #region Variables

    private RangeHandler rangeHandler;
    private CharMovement movement;

    [UnityEngine.Header("Settings")]
    [SerializeField] private Dialog dialog;

    private bool isActive;

    #endregion

    #region Unity Methods

    private void Start()
    {
        rangeHandler = gameObject.transform.Find("Range").GetComponent<RangeHandler>();
        movement = GetComponent<CharMovement>();
    }

    protected override void Update()
    {
        base.Update();

        // Caches DialogManager bools.
        isActive = DialogManager.instance.isActive;
        bool isTyping = DialogManager.instance.isTyping;
        bool hasBranchingDialog = DialogManager.instance.hasBranchingDialog;

        if (Input.GetButtonDown("Interact") && rangeHandler.playerInRange) //TODO: Make sure Player can't interact when 2 interactable ranges overlap!
        {
            if (CanInteract(!isTyping))
            {
                movement.direction = GameManager.Player().GetComponent<PlayerMovement>().orientation * -1;

                if (!isActive)
                    StartDialogue("Interact");
                else if (isActive && !hasBranchingDialog)
                    NextSentence("Interact");
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (CanInteract(!isTyping))
            {
                if (isActive && !hasBranchingDialog)
                    NextSentence("Cancel");
            }
        }

        if (Input.GetButtonDown("Toggle"))
            ToggleAutoAdvance();

        if (Input.GetButtonDown("Start") && isActive)
            SkipDialog();

        if (rangeHandler.playerInRange && PlayerInteraction.contextBox.activeSelf)
            SetContextText("Talk");
    }

    private void LateUpdate()
    {
        isActive = DialogManager.instance.isActive; // Caches DialogManager bools.


        if (isActive)
            CameraController.instance.ZoomCamera(4.2f, CameraController.instance.moveSpeed);
        else
            CameraController.instance.ZoomCamera(CameraController.instance.startSize, CameraController.instance.moveSpeed);

    }

    #endregion

    private void StartDialogue(string input)
    {
        DialogManager.instance.StartDialog(dialog);
    }

    private void NextSentence(string input)
    {
        DialogManager.instance.NextSentence(input);
    }

    private void SkipDialog()
    {
        StartCoroutine(DialogManager.instance.SkipDialog());
    }

    private void ToggleAutoAdvance()
    {
        if (DialogManager.instance.isActive || DialogManager.instance.isTyping)
            DialogManager.instance.autoAdvance = !DialogManager.instance.autoAdvance; // Toggles autoAdvance bool.
    }
}
