using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class DialogController : MonoBehaviour
{
    #region Fields

    private static DialogController instance;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static DialogController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogController>();
            }

            return instance;
        }
    }

    public ControllerFlags Flags { get; } = new ControllerFlags(true, false);

    #endregion

    #region Events

    public event EventHandler OnDialogEnd;
    public event EventHandler OnDialogStart;

    #endregion

    #region Nested Classes

    public class ControllerFlags
    {
        public bool isActive { get; internal set; }
        public bool isInDialog { get; internal set; }

        internal ControllerFlags(bool isActive, bool isInIDalog)
        {
            this.isActive = isActive;
            this.isInDialog = isInIDalog;
        }
    }

    #endregion

    #region Variables

    private DialogUserInterfaceController userInterfaceController;

    [SerializeField] public Dialog dialog;

    #endregion

    #region Miscellaneous Methods

    public void SetActive(bool isActive, List<Dialog.DialogData> dialog = null)
    {
        Flags.isInDialog = isActive;

        if (dialog != null)
        {
            userInterfaceController.Dialog = dialog;
        }

        if (isActive)
        {
            OnDialogStart?.Invoke(this, EventArgs.Empty);
            StartCoroutine(userInterfaceController.SetActive(isActive));
        }
        else
        { 
            OnDialogEnd?.Invoke(this, EventArgs.Empty);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        userInterfaceController = GetComponent<DialogUserInterfaceController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) // Input.GetButtonDown("Interact")
        {
            //SetActive(true, dialog.Data[0].LanguageData);
        }
    }


    #endregion
}

