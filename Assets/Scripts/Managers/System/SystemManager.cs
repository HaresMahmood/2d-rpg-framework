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
public class SystemManager : MonoBehaviour
{
    #region Variables

    public static SystemManager instance;

    public SystemUserInterface userInterface { get; private set; }

    private TestInput input = new TestInput();

    public bool isActive { get; set; } = false;

    public GameObject systemContainer { get; private set; }

    public string[] navigationNames { get; private set; } = new string[] { "Save", "Settings", "Tutorials", "Controls", "Quit" };

    public int selectedNavOption { get; private set; }
    private int totalNavOptions;
    public Flags flags = new Flags(true, false, false);

    #endregion

    #region Structs

    public struct Flags
    {
        public bool isInNavigation { get; set; }
        public bool isInSettings { get; set; }
        public bool isSaving { get; set; }

        public Flags(bool isInNavigation, bool isInSettings,bool isSaving)
        {
            this.isInNavigation = isInNavigation;
            this.isInSettings = isInSettings;
            this.isSaving = isSaving;
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

    private IEnumerator EnableSettings()
    {
        flags.isInNavigation = false;
        flags.isInSettings = true;
        StartCoroutine(userInterface.AnimateNavigation("isInSettings", false));
        userInterface.AnimateNavigationOption(selectedNavOption, 0);
        selectedNavOption = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(SettingsManager.instance.InitializeSettings());
    }

    private void GetInput()
    {
        if (isActive)
        {
            if (flags.isInNavigation)
            {
                bool hasInput;
                (selectedNavOption, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, userInterface.navOptions.Length, selectedNavOption);
                if (hasInput)
                {
                    userInterface.AnimateNavigationOption(selectedNavOption, (int)Input.GetAxisRaw("Vertical"));
                    //input.OnUserInput += SystemManager_OnUserInput;
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
                        flags.isInNavigation = false;
                        flags.isSaving = true;
                        StartCoroutine(userInterface.AnimateNavigation("isSaving", false));
                        userInterface.AnimateNavigationText(selectedNavOption, 185, 0.2f);
                    }
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
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        systemContainer = PauseManager.instance.pauseContainer.transform.Find("System").gameObject;
        userInterface = systemContainer.GetComponent<SystemUserInterface>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (PauseManager.instance.isPaused)
        {
            GetInput();
        }
    }

    #endregion
}
