using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class SettingsCategoryUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => settings.Count;

    #endregion

    #region Variables

    private List<SettingUserInterface> settings;

    private Scrollbar scrollBar;
    private TextMeshProUGUI descriptionText;

    #endregion

    #region MIscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment)
    {
        StartCoroutine(UpdateSelector(settings[selectedValue].transform.Find("Value")));
    }

    public IEnumerator FadePanel(float opacity, float animationDuration)
    {
        if (opacity == 1)
        {
            StartCoroutine(descriptionText.gameObject.FadeOpacity(1f, animationDuration));
            UpdateSelector(settings[0].transform);
        }

        StartCoroutine(gameObject.FadeOpacity(opacity, animationDuration));

        if (opacity == 0)
        {
            yield return new WaitForSecondsRealtime(animationDuration);
            gameObject.SetActive(false);
        }
    }

    public void ActivatePanel(float opacity, float animationDuration = 0.1f)
    {
        StartCoroutine(UpdateSelector(opacity == 1f ? settings[0].transform.Find("Value") : null));
        StartCoroutine(descriptionText.gameObject.FadeOpacity(opacity, animationDuration));

        if (scrollbar.gameObject.activeSelf)
        {
            StartCoroutine(scrollbar.gameObject.FadeOpacity(opacity, animationDuration));
        }

        StartCoroutine(FadePanel(opacity, animationDuration));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        settings = GetComponentsInChildren<SettingUserInterface>().ToList();


        selector = transform.parent.Find("Selector").gameObject;
        descriptionText = transform.parent.parent.Find("Description").GetComponent<TextMeshProUGUI>();
        scrollbar = transform.parent.parent.Find("Scrollbar").GetComponent<Scrollbar>();

        base.Awake();
    }

    #endregion
}
