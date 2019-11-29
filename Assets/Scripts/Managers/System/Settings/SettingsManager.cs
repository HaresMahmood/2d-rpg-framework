﻿using System;
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

    public int selectedSetting { get; private set; } = 0;
    public int selectedNavOption { get; private set; } = 0;

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
        if (flags.isSettingSelected)
        {
            
        }
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

    public IEnumerator InitializeSettings()
    {
        userInterface.SelectSetting(0.3f, false);
        userInterface.UpdateNavigationOptions();
        selectedNavOption = 0;
        userInterface.AnimateNavigationOption(selectedNavOption, -1);
        userInterface.UpdateScrollbar();

        float waitTime = userInterface.GetAnimationTime();
        yield return new WaitForSecondsRealtime(waitTime);

        flags.isActive = true;
    }

    private void ToggleSetting(float containerOpcatity, bool setActive)
    {
        userInterface.SelectSetting(containerOpcatity, setActive);
        flags.isSettingSelected = setActive;
        selectedSetting = 0;
    }

    private void UpdateSetting(int increment, bool resetScrollbar = false)
    {
        if (!resetScrollbar)
        {
            userInterface.UpdateScrollbar(selectedSetting);
        }
        else
        {
            selectedSetting = 0;
            userInterface.UpdateScrollbar();
        }
        userInterface.UpdateIndicator();
        userInterface.UpdateSelectedSetting(selectedSetting, increment);
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
                userInterface.UpdateSettingList(selectedNavOption, (int)Input.GetAxisRaw("Vertical"));
                UpdateSetting(-1, true);
                //input.OnUserInput += SettingsManager_OnUserInput;
            }
            if (Input.GetButtonDown("Interact"))
            {
                ToggleSetting(1f, true);
                userInterface.UpdateSelectedSetting(0, -1);

            }
            if (Input.GetButtonDown("Cancel"))
            {
                //StartCoroutine(systemUserInterface.AnimateNavigation());
            }
        }
        else
        {
            bool hasInput;
            (selectedSetting, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, userInterface.settings.Length, selectedSetting);
            if (hasInput)
            {
                UpdateSetting((int)Input.GetAxisRaw("Vertical"));
                //input.OnUserInput += SettingsManager_OnUserInput;

            }
            if (Input.GetButtonDown("Toggle"))
            {
                //ToggleViewingMode(SystemManager.instance.viewingMode);
                //UpdateSettingCategory();
            }

            if (Input.GetButtonDown("Cancel"))
            {
                userInterface.UpdateSelectedSetting(selectedSetting, 0);
                ToggleSetting(0.3f, false);
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
