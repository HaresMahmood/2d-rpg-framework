using System;
using System.Collections;
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

    public List<BranchingDialog.DialogBranch> Branches { private get; set; }

    #endregion

    #region Events

    public event EventHandler<Type> OnDialogPause;

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        userInterface.FadeButtons(isActive, Branches);

        if (!isActive)
        {
            if (condition)
            {
                userInterface.InvokeButton(Branches[selectedValue]);
                GetComponent<DialogUserInterfaceController>().Dialog = Branches[selectedValue].NextDialog != null ? Branches[selectedValue].NextDialog.Data[0].LanguageData : null;
                StartCoroutine(GetComponent<DialogUserInterfaceController>().SetActive(!isActive, false)); // TODO: Debug
            }
            else
            {
                GetComponent<DialogUserInterfaceController>().Dialog = null;
            }

            UpdateSelectedObject(selectedValue, 0);
        }
        else
        {
            yield return new WaitForSeconds(0.15f);
        }

        Flags.IsActive = isActive;
    }

    protected override void GetInput(string axisName)
    {
        base.GetInput(axisName);

        if (Input.GetButtonDown("Interact"))
        {
            StartCoroutine(SetActive(false, true));
        }

        if (Input.GetButtonDown("Cancel"))
        {
            selectedValue = userInterface.MaxObjects - 1;
            StartCoroutine(SetActive(false, true));
        }

        if (Input.GetButtonDown("Toggle"))
        {
            Debug.Log("Pressed Toggle");
        }

        if (Input.GetButtonDown("Start"))
        {
            StartCoroutine(SetActive(false));
            OnDialogPause?.Invoke(this, GetType());
        }
    }

    private IEnumerator SetActive(bool isActive)
    {
        yield return null;

        Flags.IsActive = isActive;
    }

    #endregion

    #region Event Methods

    private void DialogPauseUserInterfaceController_OnDialogUnpause(object sender, Type type)
    {
        if (type == GetType())
        {
            StartCoroutine(SetActive(true));
        }
    }

    private void DialogPauseUserInterfaceController_OnDialogSkip(object sender, Type type)
    {
        if (type == GetType())
        {
            StartCoroutine(SetActive(false, false));
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        GetComponent<DialogPauseUserInterfaceController>().OnDialogDUnpause += DialogPauseUserInterfaceController_OnDialogUnpause;
        GetComponent<DialogPauseUserInterfaceController>().OnDialogSkip += DialogPauseUserInterfaceController_OnDialogSkip;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected override void Update()
    {
        if (Flags.IsActive)
        {
            GetInput("Horizontal");
        }
    }

    #endregion
}

