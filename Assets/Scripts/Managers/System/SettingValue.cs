using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private void UpdateText(string value)
    {
        valueText.SetText(value);
    }

    private void UpdateSlider(int value)
    {
        Transform slider = transform.Find("Value/Slider");
        float normalizedValue = selectedIndex / values.Count;
        float normalizedPosition = slider.Find("Knob").GetComponent<RectTransform>().sizeDelta.x * normalizedValue;
        normalizedPosition = Mathf.Clamp(normalizedPosition, (slider.Find("Knob").GetComponent<RectTransform>().sizeDelta.x / 2), -(slider.Find("Knob").GetComponent<RectTransform>().sizeDelta.x / 2));
        slider.Find("Knob").position = new Vector2(normalizedPosition, slider.Find("Knob").position.y);
    }

    #endregion

    #region Event Methods

    private void SettingValue_OnUserInput(object sender, EventArgs e)
    {
        selectedValue = values[selectedIndex];
        UpdateText(selectedValue);
        if (mode == Mode.Slider)
        {
            //UpdateSlider(selectedIndex);
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
