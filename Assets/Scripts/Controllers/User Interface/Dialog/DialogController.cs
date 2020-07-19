using System.Collections;
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

    [SerializeField] private Dialog dialog;

    #endregion

    #region Miscellaneous Methods

    public void SetActive(bool isActive, List<Dialog.DialogData> dialog)
    {
        Flags.isInDialog = isActive;

        userInterfaceController.Dialog = dialog;
        StartCoroutine(userInterfaceController.SetActive(isActive));
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Z)) // Input.GetButtonDown("Interact")
        {
            SetActive(true, dialog.Data[0].LanguageData);
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


    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (Flags.isActive)
        {
            GetInput();
        }
    }

    #endregion
}

