using System;
using System.Collections;
using System.Collections.Generic;
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

    #region Events

    public event EventHandler<Type> OnDialogPause;

    #endregion

    #region Nested Classes

    public class DialogUserInterfaceFlags : UserInterfaceFlags
    {
        public bool IsAutoAdvanceOn { get; internal set; } // TODO: Bad name
        public bool IsSentenceComplete { get; internal set; } // TODO: Bad name

        internal DialogUserInterfaceFlags(bool isActive, bool isAutoAdvanceOn, bool isSentenceComplete) : base(isActive)
        {
            IsActive = isActive;
            IsAutoAdvanceOn = isAutoAdvanceOn;
            IsSentenceComplete = isSentenceComplete;
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
                StartCoroutine(SetActive(false, true));
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
                StartCoroutine(SetActive(isActive, true));
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
                NextSentence();
            }
        }

        if (Input.GetButtonDown("Toggle"))
        {
            flags.IsAutoAdvanceOn = !flags.IsAutoAdvanceOn;

            //if (flags.IsAutoAdvanceOn && flags.IsSentenceComplete)
            //{
            //    NextSentence();
            //}
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

    private IEnumerator AutoAdvance(float waitTime = 1.5f)
    {
        yield return new WaitForSeconds(waitTime);

        NextSentence();
    }

    private void NextSentence()
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

        flags.IsSentenceComplete = false;
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
        StartCoroutine(SetActive(false, true));
    }

    private void DialogText_OnFadeComplete(object sender, System.EventArgs e)
    {
        flags.IsSentenceComplete = true;

        if (flags.IsAutoAdvanceOn)
        {
            StartCoroutine(AutoAdvance());
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

        GetComponent<DialogPauseUserInterfaceController>().OnDialogDUnpause += DialogPauseUserInterfaceController_OnDialogUnpause;
        GetComponent<DialogPauseUserInterfaceController>().OnDialogSkip += DialogPauseUserInterfaceController_OnDialogSkip;
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        userInterface.DialogText.OnFadeComplete += DialogText_OnFadeComplete;
    }

    #endregion 
}
