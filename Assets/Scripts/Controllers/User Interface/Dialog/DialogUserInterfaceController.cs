﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class DialogUserInterfaceController : UserInterfaceController
{
    #region Fields

    private static DialogUserInterfaceController instance;

    [SerializeField] private DialogUserInterface userInterface;

    private DialogUserInterfaceFlags flags = new DialogUserInterfaceFlags(false, false, false);

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static DialogUserInterfaceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogUserInterfaceController>();
            }

            return instance;
        }
    }

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    public override UserInterfaceFlags Flags
    {
        get { return flags; }
    }

    public List<Dialog.DialogData> Dialog { private get; set; }

    #endregion

    #region Variables

    private DialogController controller;

    #endregion

    #region Nested Classes

    public class DialogUserInterfaceFlags : UserInterfaceFlags
    {
        public bool IsPaused { get; internal set; }
        public bool IsAutoAdvanceOn { get; internal set; } // TODO: Bad name

        internal DialogUserInterfaceFlags(bool isActive, bool isPaused, bool isAutoAdvanceOn) : base(isActive)
        {
            IsActive = isActive;
            IsPaused = isPaused;
            IsAutoAdvanceOn = isAutoAdvanceOn;
        }
    }

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        if (isActive && condition)
        {
            selectedValue = 0;

            if (Dialog != null)
            {
                userInterface.UpdateInformation(Dialog[selectedValue]);
            }
            else
            {
                StartCoroutine(SetActive(false));
                yield break;
            }

            yield return new WaitForSeconds(0.15f);
        }
        else if (!condition)
        {
            //StartCoroutine(userInterface.ActivateBranchedPanel(!isActive));

            if (!isActive)
            {
                GetComponent<BranchingDialogUserInterfaceController>().Branches = Dialog[selectedValue].Branch.Branches;
                StartCoroutine(GetComponent<BranchingDialogUserInterfaceController>().SetActive(!isActive)); // TODO: Debug
                StartCoroutine(userInterface.ActivateBranchedPanel(!isActive));
            }
            else
            {
                StartCoroutine(SetActive(isActive));
                yield break;
            }
        }
        else if (!isActive)
        {
            controller.SetActive(isActive); // TODO: Debug
        }

        if (condition)
        {
            StartCoroutine(userInterface.ActivatePanel(isActive));
        }

        Flags.IsActive = isActive;
    }

    protected override void GetInput(string axisName)
    {
        if (Input.GetButtonDown("Interact") || Input.GetButtonDown("Cancel"))
        {
            if (!userInterface.Stop())
            {
                if (selectedValue < Dialog.Count - 1 || Dialog == null)
                {
                    selectedValue++;
                    userInterface.UpdateInformation(Dialog[selectedValue]);
                }
                else
                {
                    if (Dialog[selectedValue].Branch == null)
                    {
                        StartCoroutine(userInterface.ActivatePanel(false));
                        controller.SetActive(false);
                    }
                }
            }

            //Debug.Log("Pressed Interact");
        }

        if (Input.GetButtonDown("Toggle"))
        {
            Debug.Log("Pressed Toggle");
        }

        if (Input.GetButtonDown("Start"))
        {
            flags.IsPaused = !flags.IsPaused;

            userInterface.PauseDialog(flags.IsPaused);
        }

        if (Input.GetButtonDown("Remove") && flags.IsPaused)
        {
            flags.IsPaused = false;

            userInterface.PauseDialog(flags.IsPaused);
            StartCoroutine(userInterface.ActivatePanel(false));
            controller.SetActive(false);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        controller = GetComponent<DialogController>();
    }

    #endregion 
}
