﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class BranchingDialogUserInterfaceController : UserInterfaceController
{
    #region Fields

    private static BranchingDialogUserInterfaceController instance;

    [SerializeField] private BranchingDialogUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static BranchingDialogUserInterfaceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BranchingDialogUserInterfaceController>();
            }

            return instance;
        }
    }

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    public List<BranchingDialog.DialogBranch> Branch { private get; set; }

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        userInterface.FadeButtons(isActive);

        if (!isActive)
        {
            GetComponent<DialogUserInterfaceController>().Dialog = Branch[selectedValue].NextDialog != null ? Branch[selectedValue].NextDialog.Data[0].LanguageData : null;
            StartCoroutine(GetComponent<DialogUserInterfaceController>().SetActive(!isActive, false)); // TODO: Debug
        }
        else
        {
            yield return new WaitForSeconds(0.15f);
        }

        Flags.isActive = isActive;
    }

    protected override void GetInput(string axisName)
    {
        if (Input.GetButtonDown("Interact"))
        {
            StartCoroutine(SetActive(false));
            Debug.Log("Pressed Interact");
        }

        if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(SetActive(false));
            Debug.Log("Pressed Caccel");
        }

        if (Input.GetButtonDown("Toggle"))
        {
            Debug.Log("Pressed Toggle");
        }

        if (Input.GetButtonDown("Start"))
        {
            Debug.Log("Pressed Start");
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected override void Update()
    {
        if (Flags.isActive)
        {
            GetInput("Horizontal");
        }
    }

    #endregion
}

