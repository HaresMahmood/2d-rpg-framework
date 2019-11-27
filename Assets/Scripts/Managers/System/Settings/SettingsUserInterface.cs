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

    private GameObject generalSettings, customizationSettings, activeSettings, indicator;
    public Transform[] navOptions { get; private set; }
    private Transform[] originalSettings, settings;
    private Scrollbar scrollBar;
    private TextMeshProUGUI descriptionText;

    #endregion

    #region Miscellaneous Methods

    public void AnimateSettings()
    {
        if (GetComponent<CanvasGroup>().alpha == 0)
        {
            StartCoroutine(gameObject.FadeOpacity(0.5f, 0.3f));
        }
        else
        {
            StartCoroutine(gameObject.FadeOpacity(0f, 0.3f));
        }
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

    private void UpdateSettingList()
    {
        originalSettings = activeSettings.transform.GetChildren();
        originalSettings = originalSettings.Where(val => val != null && val.name != "Text").ToArray();
        settings = originalSettings;
        //SettingsManager.instance.selectedSetting = 0;
    }

    public void UpdateSettingCategory()
    {
        //UpdateSettingList();
        float settingTotal = (float)settings.Length;
        float targetValue = 1.0f - (float)SettingsManager.instance.selectedSetting / (settingTotal - 1);
        StartCoroutine(scrollBar.LerpScrollbar(targetValue, 0.08f));

        descriptionText.SetText(settings[SettingsManager.instance.selectedSetting].GetComponent<SettingValue>().GetDescription());
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        generalSettings = transform.Find("General").gameObject;
        customizationSettings = transform.Find("Customization").gameObject;
        indicator = transform.Find("Indicator").gameObject;

        navOptions = transform.parent.transform.Find("Navigation/Options").GetChildren();

        descriptionText = transform.parent.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        scrollBar = transform.Find("Scrollbar").GetComponent<Scrollbar>();

        activeSettings = generalSettings;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
