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
public class SettingsUserInterface : MonoBehaviour
{
    #region Variables

    public event EventHandler OnSettingSelected = delegate { };

    private GameObject indicator;
    public Transform[] settingCategories;
    public Transform[] navOptions { get; private set; }
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

    public void AnimateNavigationOption(int selectedOption, int increment)
    {
        int previousOption = ExtensionMethods.IncrementCircularInt(selectedOption, navOptions.Length, increment);

        navOptions[selectedOption].GetComponent<Animator>().SetBool("isSelected", true);
        StartCoroutine(navOptions[selectedOption].GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
        navOptions[previousOption].GetComponent<Animator>().SetBool("isSelected", false);
        StartCoroutine(navOptions[previousOption].GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(Color.white, 0.1f));
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

        settings = settingCategories[selectedNavOption].GetChildren().Where(val => val != null && val.name != "Text").ToArray();
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

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        indicator = transform.Find("Indicator").gameObject;

        navOptions = transform.parent.transform.Find("Navigation/Options").GetChildren();

        descriptionText = transform.parent.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        scrollBar = transform.Find("Scrollbar").GetComponent<Scrollbar>();

        UpdateSettingList(0, -1);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
