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
    public Transform[] settings { get; private set; }
    private Transform[] originalSettings;
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

    private void UpdateSettingList()
    {
        originalSettings = activeSettings.transform.GetChildren();
        originalSettings = originalSettings.Where(val => val != null && val.name != "Text").ToArray();
        settings = originalSettings;
        //SettingsManager.instance.selectedSetting = 0;
    }

    public void UpdateSettings()
    {
        float settingTotal = (float)settings.Length;
        float targetValue = 1.0f - (float)SettingsManager.instance.selectedSetting / (settingTotal - 1);
        StartCoroutine(scrollBar.LerpScrollbar(targetValue, 0.08f));

        indicator.transform.position = settings[SettingsManager.instance.selectedSetting].Find("Value").position;

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

        settings = activeSettings.transform.GetChildren().Where(val => val != null && val.name != "Text").ToArray();

        //UpdateSettingList();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
