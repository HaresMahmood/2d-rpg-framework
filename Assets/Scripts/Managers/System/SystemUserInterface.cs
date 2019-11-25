using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class SystemUserInterface : MonoBehaviour
{
    #region Variables

    public event EventHandler OnSettingSelected = delegate { };

    private TestInput input = new TestInput();

    private GameObject navContainer, generalSettings, customizationSettings, activeSettings, indicator;
    private Transform[] navOptions, originalSettings, settings;
    private Scrollbar scrollBar;
    private TextMeshProUGUI descriptionText;

    [HideInInspector] public int selectedNavOption, totalNavOptions;
    private int selectedSetting = -1, totalSettingOptions;

    //private bool isInSettings = false, isSettingSelected = false;
    private Flags flags = new Flags(false, false);

    private struct Flags
    {
        public bool isInSettings { get; set; }
        public bool isSettingSelected { get; set; }
        /*
        {
            get { return isSettingSelected; }
            set { if (isSettingSelected) isSettingSelected = value; }
        }
        */

        public Flags(bool isInSettings, bool isSettingSelected)
        {
            this.isInSettings = isInSettings;
            this.isSettingSelected = isSettingSelected;
        }
    }

    #endregion

    #region Event Methods

    private void SystemManager_OnUserInput(object sender, EventArgs e)
    {
        if (!flags.isSettingSelected)
        {
            AnimateNavigation();
            foreach (Transform setting in settings)
            {
                setting.GetComponent<SettingValue>().SetStatus(false);
            }
            if (flags.isInSettings)
            {
                if (string.Equals(navOptions[selectedNavOption].name, "Customization"))
                {
                    transform.Find("Settings").GetComponent<ScrollRect>().content = customizationSettings.GetComponent<RectTransform>();
                    generalSettings.SetActive(false);
                    customizationSettings.SetActive(true);
                    activeSettings = customizationSettings;
                    UpdateSettingList();
                }
                else
                {
                    transform.Find("Settings").GetComponent<ScrollRect>().content = generalSettings.GetComponent<RectTransform>();
                    generalSettings.SetActive(true);
                    customizationSettings.SetActive(false);
                    activeSettings = generalSettings;
                    UpdateSettingList();
                }
            }
        }
        else
        {
            UpdateSettingCategory();
        }

        input.OnUserInput -= SystemManager_OnUserInput;
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
        //selectedSettingValue = 0;
        scrollBar.value = 1;
    }

    private void UpdateSettingCategory()
    {
        //UpdateSettingList();
        float settingTotal = (float)settings.Length;
        float targetValue = 1.0f - (float)selectedSetting / (settingTotal - 1);
        StartCoroutine(scrollBar.LerpScrollbar(targetValue, 0.08f));

        descriptionText.SetText(settings[selectedSetting].GetComponent<SettingValue>().GetDescription());
    }

    private void DrawSettings()
    {
        string[] newText;
        if (flags.isInSettings)
        {
            newText = SystemManager.instance.settingsText;
        }
        else
        {
            newText = SystemManager.instance.highlevelText;
        }

        foreach (Transform option in navOptions)
        {
            option.GetComponentInChildren<TextMeshProUGUI>().SetText(newText[Array.IndexOf(navOptions, option)]);
            option.name = newText[Array.IndexOf(navOptions, option)];
        }
    }

    private IEnumerator AnimateOptions()
    {
        if (!flags.isInSettings)
        {
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInHighlevel", false);
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInSettings", true);
            StartCoroutine(generalSettings.transform.parent.gameObject.FadeOpacity(0.5f, 0.3f));
            StartCoroutine(descriptionText.gameObject.FadeOpacity(1f, 0.3f));
            selectedSetting = -1;
            flags.isInSettings = true;
        }
        else
        {
            flags.isInSettings = false;
            selectedSetting = 0;
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInHighlevel", true);
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInSettings", false);
            StartCoroutine(generalSettings.transform.parent.gameObject.FadeOpacity(0f, 0.3f));
            StartCoroutine(descriptionText.gameObject.FadeOpacity(0f, 0.3f));
        }

        yield return new WaitForSecondsRealtime(navContainer.transform.Find("Options").GetComponent<Animator>().GetAnimationTime() / 2);
        selectedNavOption = 0;
        AnimateNavigation();
        DrawSettings();
    }

    private void AnimateNavigation()
    {
        foreach (Transform option in navOptions)
        {
            if (Array.IndexOf(navOptions, option) == selectedNavOption)
            {
                option.GetComponent<Animator>().SetBool("isSelected", true);
                StartCoroutine(option.GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
            }
            else
            {
                option.GetComponent<Animator>().SetBool("isSelected", false);
                StartCoroutine(option.GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(Color.white, 0.1f));
            }
        }
    }

    private void GetInput()
    {
        if (!PauseManager.instance.inPartyMenu && SystemManager.instance.isActive)
        {
            if (!flags.isSettingSelected)
            {
                bool hasInput;
                (selectedNavOption, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, totalNavOptions, selectedNavOption);
                if (hasInput)
                {
                    input.OnUserInput += SystemManager_OnUserInput;
                }
                if (Input.GetButtonDown("Interact"))
                {
                    if (!flags.isInSettings)
                    {
                        StartCoroutine(AnimateOptions());
                    }
                    else
                    {
                        indicator.SetActive(true);
                        StartCoroutine(generalSettings.transform.parent.gameObject.FadeOpacity(1f, 0.3f));
                        selectedSetting = 0;
                        flags.isSettingSelected = true;
                    }
                }
                if (Input.GetButtonDown("Cancel") && flags.isInSettings)
                {
                    StartCoroutine(AnimateOptions());
                }
            }
            else
            {
                bool hasInput;
                (selectedSetting, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, totalSettingOptions, selectedSetting);
                if (hasInput)
                {
                    input.OnUserInput += SystemManager_OnUserInput;
                }

                if (Input.GetButtonDown("Toggle"))
                {
                    SystemManager.instance.viewingMode = (SystemManager.ViewingMode)ExtensionMethods.IncrementCircularInt((int)SystemManager.instance.viewingMode, Enum.GetNames(typeof(SystemManager.ViewingMode)).Length, 1);
                    ToggleViewingMode(SystemManager.instance.viewingMode);
                    UpdateSettingCategory();
                }

                if (Input.GetButtonDown("Cancel"))
                {
                    indicator.SetActive(false);
                    StartCoroutine(generalSettings.transform.parent.gameObject.FadeOpacity(0.5f, 0.3f));
                    selectedSetting = -1;
                    flags.isSettingSelected = false;
                }
            }
        }
    }

    private void UpdateSettingList()
    {
        originalSettings = activeSettings.transform.GetChildren();
        originalSettings = originalSettings.Where(val => val != null && val.name != "Text").ToArray();
        settings = originalSettings;
        selectedSetting = 0;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        navContainer = transform.Find("Navigation").gameObject;
        generalSettings = transform.Find("Settings/General").gameObject;
        customizationSettings = transform.Find("Settings/Customization").gameObject;
        indicator = transform.Find("Settings/Indicator").gameObject;

        navOptions = navContainer.transform.Find("Options").GetChildren();

        descriptionText = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        scrollBar = transform.Find("Settings/Scrollbar").GetComponent<Scrollbar>();

        activeSettings = generalSettings;

        UpdateSettingList();

        totalNavOptions = navOptions.Length; // Debug

        selectedNavOption = 0;
        indicator.SetActive(false);
        ToggleViewingMode(SystemManager.instance.viewingMode);
        AnimateNavigation();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (PauseManager.instance.isPaused)
        {
            GetInput();
            //AnimateNavigation();

            totalSettingOptions = settings.Length;
            if (selectedSetting >= 0)
            {
                if (settings[selectedSetting].childCount > 0)
                {
                    indicator.transform.position = settings[selectedSetting].Find("Value").position;
                }
            }

            if (selectedSetting >= 0)
            {
                foreach (Transform setting in settings)
                {
                    if (Array.IndexOf(settings, setting) != selectedSetting)
                    {
                        setting.GetComponent<SettingValue>().SetStatus(false);
                    }
                }
                settings[selectedSetting].GetComponent<SettingValue>().SetStatus(true);
            }
            else
            {
                foreach (Transform setting in settings)
                
                    setting.GetComponent<SettingValue>().SetStatus(false);
                }
            }
        }
    }

    #endregion
}
