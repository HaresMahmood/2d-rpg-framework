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

    [UnityEngine.Header("Setup")]
    // Debug
    public GameObject indicator;
    public GameObject generalSettings;
    public Scrollbar scrollBar;
    public int selectedSetting, totalSettingOptions, totalSettingValues, selectedSettingValue;

    [UnityEngine.Header("Settings")]
    [SerializeField] private Material chartMaterial;

    [HideInInspector] public GameObject systemContainer, navContainer;//, indicator; // Debug

    private Transform[] navOptions;
    private string[] highlevelText = new string[] { "Save", "Settings", "Tutorials", "Controls", "Quit" };
    private string[] settingsText = new string[] { "General", "Battle", "Customization", "Accessability", "Test" };


    [HideInInspector] public int selectedNavOption, totalNavOptions;

    [HideInInspector] public bool isActive = false, isDrawing = false, isInSettings = false, isSettingSelected = false;
    private bool isInteracting = false;

    public event EventHandler OnSettingSelected = delegate { };

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

        selectedNavOption = 0;
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

            // Debug
            Transform[] settings = generalSettings.transform.GetChildren();
            settings = settings.Where(val => val.name != "Text").ToArray();

            float settingTotal = (float)settings.Length;
            scrollBar.value = 1.0f - (float)selectedSetting / (settingTotal - 1);
            
            totalSettingOptions = settings.Length;
            if (settings[selectedSetting].childCount > 0)
            {
                indicator.transform.position = new Vector2(indicator.transform.position.x, settings[selectedSetting].Find("Value").position.y);
            }

            totalSettingValues = settings[selectedSetting].GetComponent<SettingValue>().GetValues().Count;
            selectedSettingValue = settings[selectedSetting].GetComponent<SettingValue>().GetValues().FindIndex(value => value == settings[selectedSetting].GetComponent<SettingValue>().GetSelectedValue());
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
            isInSettings = true;
        }
        else
        {
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInHighlevel", true);
            navContainer.transform.Find("Options").GetComponent<Animator>().SetBool("isInSettings", false);
            isInSettings = false;
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
        totalNavOptions = navOptions.Length; // Debug

        if (!PauseManager.instance.inPartyMenu && isActive)
        {
            bool hasInput;
            (selectedNavOption, hasInput) = InputManager.GetInput("Vertical", InputManager.Axis.Vertical, totalNavOptions, selectedNavOption);
            if (hasInput)
            {
                GameManager.instance.transform.GetComponentInChildren<InputManager>().OnUserInput += PartyManager_OnUserInput;
            }
            if (Input.GetButtonDown("Interact"))
            {
                StartCoroutine(AnimateOptions());
            }
        }
        */


        // Debug

        if (!PauseManager.instance.inPartyMenu && isActive)
        {
            bool hasInput;
            (selectedSetting, hasInput) = InputManager.GetInput("Vertical", InputManager.Axis.Vertical, totalSettingOptions, selectedSetting);
            if (hasInput)
            {
                GameManager.instance.transform.GetComponentInChildren<InputManager>().OnUserInput += PartyManager_OnUserInput;
            }
        }
    }

    private void PartyManager_OnUserInput(object sender, EventArgs e)
    {
        //AnimateNavigation();
        GameManager.instance.transform.GetComponentInChildren<InputManager>().OnUserInput -= PartyManager_OnUserInput;
    }
}
