using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class SettingValue : MonoBehaviour
{
    #region Variables

    [SerializeField] private Mode mode;
    [SerializeField] private List<string> values = new List<string>();
    [SerializeField] private string defaultValue;
    [ReadOnly] [SerializeField] private string selectedValue;
    [ReadOnly] [SerializeField] private bool isSelected;

    private TextMeshProUGUI valueText;

    private TestInput input = new TestInput();

    [SerializeField] private int selectedIndex;

    public enum Mode
    { 
        Slider,
        Carousel
    }

    #endregion

    #region Accessor Methods

    public List<string> GetValues()
    {
        return values;
    }

    public string GetSelectedValue()
    {
        return selectedValue;
    }

    public bool GetStatus()
    {
        return isSelected;
    }

    #endregion

    #region Helper Methods

    public void SetStatus(bool status)
    {
        isSelected = status;
    }

    #endregion

    #region Miscellaneous Methods

    private void GetInput()
    {
        if (SystemManager.instance.isActive && isSelected)
        {
            bool hasInput;
            (selectedIndex, hasInput) = input.GetInput("Horizontal", TestInput.Axis.Horizontal, values.Count, selectedIndex);
            if (hasInput)
            {
                input.OnUserInput += SettingValue_OnUserInput;
            }
        }
    }

    /*
    private void UpdateSetting(string text, int index)
    {
        UpdateText(text);
        if (mode == Mode.Slider)
        {
            UpdateSlider(index);
        }
    }
    */

    private void UpdateText(string value)
    {
        valueText.SetText(value);
    }

    private void UpdateSlider(int value)
    {
        Slider slider = transform.GetComponentInChildren<Slider>();
        float totalValues = (float)(values.Count);
        float targetValue = 1f - (float)selectedIndex / (totalValues - 1);
        StartCoroutine(LerpSlider(slider, targetValue, 0.1f));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <returns></returns>
    public IEnumerator LerpSlider(Slider slider, float targetValue, float duration)
    {
        float initialValue = slider.value;

        float t = 0; // Tracks how many seconds we've been fading.
        while (t < duration) // While time is less than the duration of the fade, ...
        {
            if (Time.timeScale == 0)
                t += Time.unscaledDeltaTime;
            else
                t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration); // Turns the time into an interpolation factor between 0 and 1. 

            slider.value = Mathf.Lerp(initialValue, targetValue, blend);

            yield return null; // Wait one frame, then repeat.
        }
    }

    #endregion

    #region Event Methods

    private void SettingValue_OnUserInput(object sender, EventArgs e)
    {
        selectedValue = values[selectedIndex];
        UpdateText(selectedValue);
        if (mode == Mode.Slider)
        {
            UpdateSlider(selectedIndex);
        }
        input.OnUserInput -= SettingValue_OnUserInput;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        valueText = transform.Find("Value").GetComponentInChildren<TextMeshProUGUI>();

        if (string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(defaultValue))
        {
            selectedValue = defaultValue;
            UpdateText(selectedValue);
        }

        selectedIndex = values.FindIndex(value => value == selectedValue);
        if (mode == Mode.Slider)
        {
            UpdateSlider(selectedIndex);
        }
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        GetInput();
    }

    #endregion
}
