using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class SettingsUserInterface : MonoBehaviour
{
    #region Variables

    public event EventHandler OnSettingSelected = delegate { };

    private GameObject indicator;
    public Transform[] settingCategories;
    public Transform[] navOptions { get; private set; }
    private List<List<Transform>> originalSettings { get; set; } = new List<List<Transform>>();
    public Transform[] settings { get; private set; }
    private Scrollbar scrollBar;
    private TextMeshProUGUI descriptionText;

    #endregion

    #region Miscellaneous Methods

    public float GetAnimationTime()
    {
        float waitTime = navOptions[SettingsManager.instance.selectedNavOption].GetComponent<Animator>().GetAnimationTime();
        return waitTime;
    }

    public void SelectSetting(float containerOpacity, bool activateIndiator)
    {
        indicator.SetActive(activateIndiator);
        StartCoroutine(gameObject.FadeOpacity(containerOpacity, 0.3f));
    }

    public void AnimateNavigationText(int selectedOption, float fontSize, float duration)
    {
        StartCoroutine(navOptions[selectedOption].GetComponentInChildren<TextMeshProUGUI>().LerpTextSize(fontSize, duration));
    }

    public void AnimateNavigationOption(int selectedOption, int increment)
    {
        int previousOption = ExtensionMethods.IncrementCircularInt(selectedOption, navOptions.Length, increment);

        AnimateNavigationText(selectedOption, 120f, 0.1f);
        StartCoroutine(navOptions[selectedOption].GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
        AnimateNavigationText(previousOption, 110f, 0.1f);
        StartCoroutine(navOptions[previousOption].GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(Color.white, 0.1f));
    }

    public void ToggleViewingMode(int selectedNavOption, SettingsManager.ViewingMode mode)
    {
        indicator.SetActive(false); // Debug

        settings = originalSettings[selectedNavOption].ToArray();
        List<SettingsManager.ViewingMode> previousMode = SettingsManager.instance.GetPreviousMode(mode);
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

        // Debug
        //UpdateIndicator();
        indicator.SetActive(true);
    }

    public void UpdateNavigationOptions()
    {
        foreach (Transform option in navOptions)
        {
            option.GetComponentInChildren<TextMeshProUGUI>().SetText(SettingsManager.instance.navigationNames[Array.IndexOf(navOptions, option)]);
            option.name = SettingsManager.instance.navigationNames[Array.IndexOf(navOptions, option)];
        }
    }

    public void UpdateSettingList(int selectedNavOption, int increment)
    {
        int previousNavOption = ExtensionMethods.IncrementCircularInt(selectedNavOption, settingCategories.Length, increment);

        ScrollRect scrollRect = transform.GetComponent<ScrollRect>();
        scrollRect.content = settingCategories[selectedNavOption].GetComponent<RectTransform>();

        settingCategories[previousNavOption].gameObject.SetActive(false);
        settingCategories[selectedNavOption].gameObject.SetActive(true);

        ToggleViewingMode(selectedNavOption, SettingsManager.instance.viewingMode);
    }

    public void UpdateIndicator()
    {
        indicator.transform.position = settings[SettingsManager.instance.selectedSetting].Find("Value").position;
    }

    public void UpdateSelectedSetting(int selectedSetting, int increment)
    {
        int previousSetting = ExtensionMethods.IncrementCircularInt(selectedSetting, settings.Length, increment);

        settings[selectedSetting].GetComponent<SettingValue>().SetStatus(true);
        settings[previousSetting].GetComponent<SettingValue>().SetStatus(false);
    }

    public void UpdateScrollbar(int selectedSetting = -1)
    {
        if (selectedSetting > -1)
        {
            float settingTotal = (float)settings.Length;
            float targetValue = 1.0f - (float)selectedSetting / (settingTotal - 1);
            StartCoroutine(scrollBar.LerpScrollbar(targetValue, 0.08f));

            descriptionText.SetText(settings[selectedSetting].GetComponent<SettingValue>().GetDescription());
        }
        else
        {
            scrollBar.value = 1;
        }
    }

    public void SetSettings(int selectedNavOption)
    {
        originalSettings.Add(settingCategories[selectedNavOption].GetChildren().Where(val => val != null && val.name != "Text").ToList());
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        indicator = transform.Find("Indicator").gameObject;

        navOptions = transform.parent.Find("Navigation/Options").GetChildren();

        descriptionText = transform.parent.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        scrollBar = transform.Find("Scrollbar").GetComponent<Scrollbar>();

        foreach (Transform option in settingCategories)
        {
            SetSettings(Array.IndexOf(settingCategories, option));
        }

        UpdateSettingList(0, -1);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // Debug
        UpdateIndicator();
    }

    #endregion
}
