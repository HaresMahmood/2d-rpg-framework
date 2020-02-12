using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SaveManager : MonoBehaviour
{
    #region Variables

    public static SaveManager instance;

    [Header("Setup")]
    [SerializeField] private SaveUserInterface userInterface;

    private TestInput input = new TestInput();
    public Flags flags = new Flags(false, false, false);

    public int selectedOption { get; private set; } = 0;

    #endregion

    #region Structs

    public struct Flags
    {
        public bool isActive { get; set; }
        public bool isSaving { get; set; }
        public bool hasSaved { get; set; }

        public Flags(bool isActive, bool isSaving, bool hasSaved)
        {
            this.isActive = isActive;
            this.isSaving = isSaving;
            this.hasSaved = hasSaved;
        }
    }

    #endregion

    #region Miscellaneous Methods

    public IEnumerator InitializeSave()
    {
        StartCoroutine(userInterface.InitializeSave());

        yield return null;

        flags.isActive = true;
    }

    public void ExitSave()
    {
        userInterface.ExitSave();
        flags.isActive = false; flags.isSaving = false;
        StartCoroutine(SystemManager.instance.DisableSave());
    }

    private void GetInput()
    {
        bool hasInput;
        (selectedOption, hasInput) = input.GetInput("Horizontal", TestInput.Axis.Horizontal, userInterface.navOptions.Length, selectedOption);
        if (hasInput)
        {

        }
        if (Input.GetButtonDown("Interact"))
        {
            if (selectedOption == 0)
            {
                StartCoroutine(userInterface.AnimateOptions());
                flags.isSaving = true;
                StartCoroutine(userInterface.AnimateProgress());
            }

        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
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
        if (PauseManager.instance.flags.isActive && SystemManager.instance.flags.isActive && flags.isActive)
        {
            GetInput();
        }
    }

    #endregion
}
