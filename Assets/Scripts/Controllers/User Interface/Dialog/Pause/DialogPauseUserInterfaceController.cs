using System;
using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class DialogPauseUserInterfaceController : UserInterfaceController
{
    #region Variables

    private Type dialogType;

    #endregion

    #region Events

    public event EventHandler<Type> OnDialogDUnpause;
    public event EventHandler<Type> OnDialogSkip;

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        ((DialogPauseUserInterface)UserInterface).SetActive(isActive);

        yield return null;

        Flags.IsActive = isActive;
    }

    protected override void GetInput(string axisName)
    {
        if (Input.GetButtonDown("Start"))
        {
            StartCoroutine(SetActive(false));

            OnDialogDUnpause?.Invoke(this, dialogType);
        }

        if (Input.GetButtonDown("Remove"))
        {
            StartCoroutine(SetActive(false));

            OnDialogSkip?.Invoke(this, dialogType);
        }
    }

    #endregion

    #region Event Methods

    private void DialogUserInterfaceController_OnDialogPause(object sender, Type type)
    {
        dialogType = type;

        StartCoroutine(SetActive(true));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        GetComponent<DialogUserInterfaceController>().OnDialogPause += DialogUserInterfaceController_OnDialogPause;
        GetComponent<BranchingDialogUserInterfaceController>().OnDialogPause += DialogUserInterfaceController_OnDialogPause;
    }

    #endregion 
}

