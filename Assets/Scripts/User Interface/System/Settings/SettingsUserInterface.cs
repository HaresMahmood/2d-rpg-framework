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
public class SettingsUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => navigation.Count;

    #endregion

    #region Variables

    List<Transform> navigation;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment)
    {
        int previousValue = ExtensionMethods.IncrementInt(selectedValue, 0, MaxObjects, increment);

        StartCoroutine(navigation[selectedValue].gameObject.FadeOpacity(0.3f, 0.15f));
        StartCoroutine(navigation[previousValue].gameObject.FadeOpacity(1f, 0.15f));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        navigation = transform.parent.Find("Navigation").GetChildren().ToList();
    }

    #endregion

    /*
    #region Variables

    public event EventHandler OnSettingSelected = delegate { };

    private GameObject indicator;
    public Transform[] settingCategories;
    public Transform[] navOptions { get; private set; }
    private List<List<Transform>> originalSettings { get; set; } = new List<List<Transform>>();
    public Transform[] settings { get; private set; }
    private Animator indicatorAnimator;
    private ScrollRect scrollRect;
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
        indicatorAnimator.enabled = activateIndiator;
        if (!activateIndiator && indicator.GetComponent<Image>().color.a != 0)
        {
            StartCoroutine(indicator.FadeOpacity(0f, 0.15f));
        }

        StartCoroutine(gameObject.FadeOpacity(containerOpacity, 0.15f));
    }

    public void AnimateNavigationText(int selectedOption, float fontSize, float duration)
    {
        StartCoroutine(navOptions[selectedOption].GetComponentInChildren<TextMeshProUGUI>().LerpTextSize(fontSize, duration));
    }

    public void AnimateNavigationOption(int selectedOption, int increment)
    {
        int previousOption = ExtensionMethods.IncrementInt(selectedOption, 0, navOptions.Length, increment);

        AnimateNavigationText(selectedOption, 120f, 0.1f);
        StartCoroutine(navOptions[selectedOption].GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(GameManager.GetAccentColor(), 0.1f));
        AnimateNavigationText(previousOption, 110f, 0.1f);
        StartCoroutine(navOptions[previousOption].GetComponentInChildren<TextMeshProUGUI>().gameObject.FadeColor(Color.white, 0.1f));
    }

    public void ToggleViewingMode(int selectedNavOption, SettingsManager.ViewingMode mode)
    {
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
    }

    public void UpdateNavigationOptions()
    {
        foreach (Transform option in navOptions)
        {
            option.GetComponentInChildren<TextMeshProUGUI>().SetText(SettingsManager.instance.navigationNames[Array.IndexOf(navOptions, option)]);
            option.name = SettingsManager.instance.navigationNames[Array.IndexOf(navOptions, option)];
        }
    }

    public void UpdateSettingList(int selectedCategory, int increment, bool animate = true)
    {
        int previousCategory = ExtensionMethods.IncrementInt(selectedCategory, 0, settingCategories.Length, increment);

        ScrollRect scrollRect = transform.GetComponent<ScrollRect>();
        scrollRect.content = settingCategories[selectedCategory].GetComponent<RectTransform>();

        if (animate)
        {
            StartCoroutine(ChangeCategory(selectedCategory, previousCategory, 0.05f));
        }
        else
        {
            settingCategories[selectedCategory].gameObject.SetActive(true);
            settingCategories[previousCategory].gameObject.SetActive(false);
        }
    }

    public IEnumerator UpdateIndicator(int selectedSetting, float waitTime)
    {
        if (SettingsManager.instance.flags.isSettingSelected)
        {
            indicatorAnimator.enabled = false;
            StartCoroutine(indicator.FadeOpacity(0f, waitTime));
            yield return new WaitForSecondsRealtime(waitTime);

            indicator.transform.position = settings[selectedSetting].Find("Value").position;
            yield return null;

            indicatorAnimator.enabled = true;
        }
        else
        {
            yield return null;
            indicator.transform.position = settings[selectedSetting].Find("Value").position;
        }
    }

    public void UpdateSelectedSetting(int selectedSetting, int increment)
    {
        int previousSetting = ExtensionMethods.IncrementInt(selectedSetting, 0, settings.Length, increment);

        settings[selectedSetting].GetComponent<SettingValue>().SetStatus(true);
        settings[previousSetting].GetComponent<SettingValue>().SetStatus(false);
    }

    private IEnumerator ChangeCategory(int selectedCategory, int previousCategory, float waitTime)
    {
        indicatorAnimator.enabled = false;
        StartCoroutine(indicator.FadeOpacity(0f, waitTime));
        FadeCategory(settingCategories[previousCategory], 0f, waitTime);
        StartCoroutine(scrollBar.gameObject.FadeOpacity(0f, waitTime));
        yield return new WaitForSecondsRealtime(waitTime);
        settingCategories[previousCategory].gameObject.SetActive(false);
        scrollRect.enabled = false;

        ToggleViewingMode(selectedCategory, SettingsManager.instance.viewingMode);

        yield return new WaitForSecondsRealtime(waitTime * 2);
        settingCategories[selectedCategory].gameObject.SetActive(true);

        scrollRect.enabled = true;
        yield return null; UpdateScrollbar();

        FadeCategory(settingCategories[selectedCategory], 1f, waitTime);
        StartCoroutine(scrollBar.gameObject.FadeOpacity(1f, waitTime));
        if (SettingsManager.instance.flags.isSettingSelected)
        {
            yield return new WaitForSecondsRealtime(waitTime); indicatorAnimator.enabled = true;
        }
    }

    private void FadeCategory(Transform category, float opacity, float duration)
    {
        StartCoroutine(category.gameObject.FadeOpacity(opacity, duration));
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

        indicatorAnimator = indicator.GetComponent<Animator>();
        descriptionText = transform.parent.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        scrollRect = transform.GetComponent<ScrollRect>();
        scrollBar = transform.Find("Scrollbar").GetComponent<Scrollbar>();

        foreach (Transform option in settingCategories)
        {
            SetSettings(Array.IndexOf(settingCategories, option));
        }

        UpdateSettingList(0, -1);
    }


    /*
    // Debug
    private void Update()
    {
        StartCoroutine(UpdateIndicator(SettingsManager.instance.selectedSetting, 0.1f));
    }

    #endregion
    */
}
