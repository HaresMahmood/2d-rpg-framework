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

    [UnityEngine.Header("Setup (debug)")]
    // Debug
    public GameObject indicator;
    public GameObject generalSettings;
    public Scrollbar scrollBar;
    public int selectedSetting, totalSettingOptions, totalSettingValues, selectedSettingValue;
    public TextMeshProUGUI descriptionText;

    [UnityEngine.Header("Settings")]
    [SerializeField] private Material chartMaterial;

    [UnityEngine.Header("Values")]
    [SerializeField] private ViewingMode viewingMode;

    [HideInInspector] public GameObject systemContainer, navContainer;//, indicator; // Debug

    private Transform[] navOptions, originalSettings, settings;
    private string[] highlevelText = new string[] { "Save", "Settings", "Tutorials", "Controls", "Quit" };
    private string[] settingsText = new string[] { "General", "Battle", "Customization", "Accessability", "Test" };


    [HideInInspector] public int selectedNavOption, totalNavOptions;

    [HideInInspector] public bool isActive = false, isDrawing = false, isInSettings = false, isSettingSelected = false;

    public event EventHandler OnSettingSelected = delegate { };
    private TestInput input = new TestInput();

    public enum ViewingMode
    {
        Basic,
        Intermediate,
        Advanced
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
        navContainer = systemContainer.transform.Find("Navigation").gameObject;

        navOptions = navContainer.transform.Find("Options").GetChildren();

        originalSettings = generalSettings.transform.GetChildren();
        originalSettings = originalSettings.Where(val => val.name != "Text").ToArray();
        settings = originalSettings;

        selectedNavOption = 0;
        ToggleCategory(viewingMode);
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
        }
    }

    #endregion

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

    private IEnumerator AnimateOptions()
    {
        if (!isInSettings)
        {
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInHighlevel", false);
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInSettings", true);
            StartCoroutine(generalSettings.transform.parent.gameObject.FadeOpacity(1f, 0.3f));
            StartCoroutine(descriptionText.gameObject.FadeOpacity(1f, 0.3f));
            isInSettings = true;
        }
        else
        {
            isInSettings = false;
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

    private void DrawSettings()
    {
        string[] newText;
        if (isInSettings)
        {
            newText = settingsText;
        }
        else
        {
            newText = highlevelText;
        }

        foreach (Transform option in navOptions)
        {
            option.GetComponentInChildren<TextMeshProUGUI>().SetText(newText[Array.IndexOf(navOptions, option)]);
        }
    }

    private void GetInput()
    {
        /*
        if (!PauseManager.instance.inPartyMenu && isActive)
        {

        }
        */

        // Debug

        if (!PauseManager.instance.inPartyMenu && isActive)
        {
            if (!isInSettings)
            {
                totalNavOptions = navOptions.Length; // Debug

                bool hasInput;
                (selectedNavOption, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, totalNavOptions, selectedNavOption);
                if (hasInput)
                {
                    input.OnUserInput += PartyManager_OnUserInput;
                }
                if (Input.GetButtonDown("Interact"))
                {
                    StartCoroutine(AnimateOptions());
                }
            }
            if (isInSettings)
            {
                bool hasInput;
                (selectedSetting, hasInput) = input.GetInput("Vertical", TestInput.Axis.Vertical, totalSettingOptions, selectedSetting);
                if (hasInput)
                {
                    input.OnUserInput += PartyManager_OnUserInput;
                }
                if (Input.GetButtonDown("Toggle"))
                {
                    viewingMode = (ViewingMode)ExtensionMethods.IncrementCircularInt((int)viewingMode, Enum.GetNames(typeof(ViewingMode)).Length, 1);
                    ToggleCategory(viewingMode);
                    UpdateSettingCategory();
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    StartCoroutine(AnimateOptions());
                }
            }
        }
    }

    private void ToggleCategory(ViewingMode mode)
    {
        settings = originalSettings;
        List<ViewingMode> previousMode = GetPreviousMode(mode);
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
        selectedSettingValue = 0;
    }

    private void UpdateSettingCategory()
    {
        // Debug
        float settingTotal = (float)settings.Length;
        float targetValue = 1.0f - (float)selectedSetting / (settingTotal - 1);
        StartCoroutine(scrollBar.LerpScrollbar(targetValue, 0.08f));

        totalSettingOptions = settings.Length;
        if (settings[selectedSetting].childCount > 0)
        {
            indicator.transform.position = settings[selectedSetting].Find("Value").position;
        }

        totalSettingValues = settings[selectedSetting].GetComponent<SettingValue>().GetValues().Count;
        selectedSettingValue = settings[selectedSetting].GetComponent<SettingValue>().GetValues().FindIndex(value => value == settings[selectedSetting].GetComponent<SettingValue>().GetSelectedValue());
        foreach (Transform setting in settings)
        {
            if (Array.IndexOf(settings, setting) != selectedSettingValue)
            {
                setting.GetComponent<SettingValue>().SetStatus(false);
            }
        }
        settings[selectedSetting].GetComponent<SettingValue>().SetStatus(true);

        descriptionText.SetText(settings[selectedSetting].GetComponent<SettingValue>().GetDescription());
        //
    }

    private List<ViewingMode> GetPreviousMode(ViewingMode category)
    {
        ViewingMode previousCategory = (ViewingMode)ExtensionMethods.IncrementCircularInt((int)category, Enum.GetNames(typeof(ViewingMode)).Length, -1);
        List<ViewingMode> previousCategories = new List<ViewingMode>();
        if (previousCategory < category)
        {
            previousCategories.AddRange(GetPreviousMode(previousCategory));
        }
        previousCategories.Add(category);
        return previousCategories;
    }

    private void PartyManager_OnUserInput(object sender, EventArgs e)
    {
        if (!isInSettings)
        {
            AnimateNavigation();
        }
        


        if (isInSettings)
        {
            UpdateSettingCategory();
        }
        input.OnUserInput -= PartyManager_OnUserInput;
    }
}
