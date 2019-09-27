﻿using UnityEngine;

public class CharInteraction : InteractableObject
{
    #region Variables

    private CharMovement movement;

    [UnityEngine.Header("Settings")]
    [SerializeField] private Dialog dialog;

    private bool isActive;

    #endregion

    #region Unity Methods

    private void Start()
    {
        movement = GetComponent<CharMovement>();
    }

    private void Update()
    {
        // Caches DialogManager bools.
        isActive = DialogManager.instance.isActive;
        bool isTyping = DialogManager.instance.isTyping;
        bool hasBranchingDialog = DialogManager.instance.hasBranchingDialog;

        if (Input.GetButtonDown("Interact"))
        {
            if (CanInteract(!isTyping))
            {
                movement.direction = GameManager.Player().GetComponent<PlayerMovement>().orientation * -1;

                if (!isActive)
                    StartDialogue();
                else if (isActive && !hasBranchingDialog)
                    NextSentence();
            }
        }

        if (Input.GetButtonDown("Toggle"))
            ToggleAutoAdvance();

        if (Input.GetButtonDown("Cancel") && isActive)
            SkipDialog();
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

    private void StartDialogue()
    {
        DialogManager.instance.StartDialog(dialog);
    }

    private void NextSentence()
    {
        DialogManager.instance.NextSentence();
    }
}
