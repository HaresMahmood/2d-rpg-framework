using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class SettingsManager : MonoBehaviour
{
    #region Variables

    public static SettingsManager instance;

    private GameObject settingsContainer;

    private TestInput input = new TestInput();

    private SettingsUserInterface userInterface;

    public string[] navigationNames { get; private set; } = new string[] { "General", "Battle", "Customization", "Accessability", "Test" };

    public int selectedSetting { get; private set; } = -1;
    private int selectedNavOption, totalSettingOptions;

    public Flags flags = new Flags(false, false);

    #endregion

    #region Structs

    public struct Flags
    {
        public bool isActive { get; set; }
        public bool isSettingSelected { get; set; }
        /*
        {
            get { return isSettingSelected; }
            set { if (isSettingSelected) isSettingSelected = value; }
        }
        */

        public Flags(bool isActive, bool isSettingSelected)
        {
            this.isActive = isActive;
            this.isSettingSelected = isSettingSelected;
        }
    }

    #endregion

    #region Event Methods

    private void SettingsManager_OnUserInput(object sender, EventArgs e)
    {
        input.OnUserInput -= SettingsManager_OnUserInput;
    }

    #endregion


    #region Miscellaneous Methods

    private List<SystemManager.ViewingMode> GetPreviousMode(SystemManager.ViewingMode category)
    {
        SystemManager.ViewingMode previousCategory = (SystemManager.ViewingMode)ExtensionMethods.IncrementCircularInt((int)category, Enum.GetNames(typeof(SystemManager.ViewingMode)).Length, -1);
        List<SystemManager.ViewingMode> previousCategories = new List<SystemManager.ViewingMode>();
        if (previousCategory < category)
        {
            previousCategories.AddRange(GetPreviousMode(previousCategory));
        }
        previousCategories.Add(category);
        return previousCategories;
    }

    /*
    private void ToggleViewingMode(SystemManager.ViewingMode mode)
    {
        settings = originalSettings;
        List<SystemManager.ViewingMode> previousMode = GetPreviousMode(mode);
        List<Transform> values = new List<Transform>();

        foreach (Transform setting in settings)
        {
            if (previousMode.Contains(setting.GetComponent<SettingValue>().GetViewingMode()))
            {
                values.Add(setting);
                setting.gameObject.SetActive(true);
            }
            else
            {
                setting.gameObject.SetActive(false);
            }
        }

        settings = values.ToArray();
        scrollBar.value = 1;
    }
    */

    public void InitializeSettings()
    {
        userInterface.AnimateSettings();
        userInterface.UpdateNavigationOptions();
        selectedNavOption = 0;
        userInterface.AnimateNavigationOption(selectedNavOption, -1);
        flags.isActive = true;
    }

    private void GetInput()
    {
        if (!flags.isSettingSelected)
        {
            bool hasInput;
            (selectedNavOption, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, userInterface.navOptions.Length, selectedNavOption);
            if (hasInput)
            {
                userInterface.AnimateNavigationOption(selectedNavOption, (int)Input.GetAxisRaw("Vertical"));
                //input.OnUserInput += SettingsManager_OnUserInput;
            }
            if (Input.GetButtonDown("Interact"))
            {
                //indicator.SetActive(true);
                //StartCoroutine(generalSettings.transform.parent.gameObject.FadeOpacity(1f, 0.3f));
                selectedSetting = 0;
            }
            if (Input.GetButtonDown("Cancel"))
            {
                //StartCoroutine(systemUserInterface.AnimateNavigation());
            }
        }
        else
        {
            bool hasInput;
            (selectedSetting, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, totalSettingOptions, selectedSetting);
            if (hasInput)
            {
                //input.OnUserInput += SettingsManager_OnUserInput;
            }

            if (Input.GetButtonDown("Toggle"))
            {
                //ToggleViewingMode(SystemManager.instance.viewingMode);
                //UpdateSettingCategory();
            }

            if (Input.GetButtonDown("Cancel"))
            {
                //indicator.SetActive(false);
                //StartCoroutine(generalSettings.transform.parent.gameObject.FadeOpacity(0.5f, 0.3f));
                selectedSetting = -1;
                flags.isSettingSelected = false;
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
        settingsContainer = PauseManager.instance.pauseContainer.transform.Find("System/Settings").gameObject;
        userInterface = settingsContainer.GetComponent<SettingsUserInterface>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (PauseManager.instance.isPaused && SystemManager.instance.isActive && flags.isActive)
        {
            GetInput();
        }
    }

    #endregion
}
