using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class SystemUserInterfaceController : UserInterfaceController
{
    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        StartCoroutine(GetComponent<SettingsUserInterfaceController>().SetActive(isActive));
        yield return null;
        StartCoroutine(base.SetActive(isActive, condition));
    }

    /*
    #region Constants

    public readonly string[] navigationNames = new string[] { "Save", "Settings", "Tutorials", "Controls", "Quit" };

    #endregion

    #region Variables

    public static SystemManager instance;

    [Header("Setup")]
    [SerializeField] private SystemUserInterface userInterface;
    public List<PanelButton> buttons = new List<PanelButton>();

    [Header("Values")]

    private TestInput input = new TestInput();
    public Flags flags = new Flags(false, true, false, false);

    public int selectedNavOption { get; private set; }

    #endregion

    #region Structs

    public struct Flags
    {
        public bool isActive { get; set; }
        public bool isInNavigation { get; set; }
        public bool isInSettings { get; set; }
        public bool isInSave { get; set; }

        public Flags(bool isActive, bool isInNavigation, bool isInSettings,bool isInSave)
        {
            this.isActive = isActive;
            this.isInNavigation = isInNavigation;
            this.isInSettings = isInSettings;
            this.isInSave = isInSave;
        }
    }

    #endregion

    #region Event Methods

    private void SystemManager_OnUserInput(object sender, EventArgs e)
    {
        input.OnUserInput -= SystemManager_OnUserInput;
    }

    #endregion

    #region Miscellaneous Methods

    private IEnumerator EnableSave()
    {
        flags.isInNavigation = false;
        flags.isInSave = true;
        StartCoroutine(userInterface.AnimateNavigation("isInSave", false));
        userInterface.AnimateNavigationText(selectedNavOption, 185, 0.2f);
        //userInterface.AnimateNavigationOption(selectedNavOption, 0);
        selectedNavOption = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(SaveManager.instance.InitializeSave());
    }

    public IEnumerator DisableSave()
    {
        StartCoroutine(userInterface.AnimateNavigation("isInSave", true));
        userInterface.AnimateNavigationText(selectedNavOption, 120, 0.2f);
        selectedNavOption = 0;

        yield return null;

        flags.isInNavigation = true;
        flags.isInSave = false;
    }

    private IEnumerator EnableSettings()
    {
        flags.isInNavigation = false;
        flags.isInSettings = true;
        StartCoroutine(userInterface.AnimateNavigation("isInSettings", false));
        userInterface.AnimateNavigationOption(selectedNavOption, 0);
        selectedNavOption = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(SettingsManager.instance.OnActive());
    }

    public IEnumerator DisableSettings()
    {
        StartCoroutine(userInterface.AnimateNavigation("isInSettings", true));

        selectedNavOption = 0;

        yield return new WaitForSecondsRealtime(0.1f);

        userInterface.AnimateNavigationOption(selectedNavOption, -1);
        userInterface.UpdateNavigation();
        StartCoroutine(FindObjectOfType<BottomPanelUserInterface>().ChangePanelButtons(buttons));
        flags.isInSettings = false;
        flags.isInNavigation = true;
    }

    private void GetInput()
    {
        if (flags.isInNavigation)
        {
            bool hasInput;
            (selectedNavOption, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, userInterface.navOptions.Length, selectedNavOption);
            if (hasInput)
            {
                userInterface.AnimateNavigationOption(selectedNavOption, (int)Input.GetAxisRaw("Vertical"));
            }

            if (Input.GetButtonDown("Interact"))
            {
                // TODO: Execute Unity-event code to determine what nav option is selected;
                if (navigationNames[selectedNavOption].Equals("Settings"))
                {
                    StartCoroutine(EnableSettings());
                }
                else
                {
                    StartCoroutine(EnableSave());
                }
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
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (PauseManager.instance.flags.isActive && flags.isActive)
        {
            GetInput();
        }


    }

    #endregion
    */
}
