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

    [SerializeField] private Mode type;
    [SerializeField] private SettingsManager.ViewingMode viewingMode;
    [SerializeField] private List<string> values = new List<string>();
    [SerializeField] private string defaultValue;
    [TextArea(1,2)] [SerializeField] private string description;
    [ReadOnly] [SerializeField] private string selectedValue;
    [ReadOnly] [SerializeField] private bool isSelected;
    [ReadOnly] [SerializeField] private bool isDirty;

    private TextMeshProUGUI valueText;

    private TestInput input = new TestInput();

    private int selectedIndex;



    #region Enums

    public enum Mode
    { 
        Slider,
        Carousel,
        Color
    }

    public enum Type
    {
        Slider,
        Carousel,
        Color
    }

    #endregion

    #endregion

    #region Accessor Methods

    public SettingsManager.ViewingMode GetViewingMode()
    {
        return viewingMode;
    }

    public List<string> GetValues()
    {
        return values;
    }

    public string GetSelectedValue()
    {
        return selectedValue;
    }

    public string GetDescription()
    {
        return description;
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
            if (type != Mode.Color)
            {
                bool hasInput;
                (selectedIndex, hasInput) = input.GetInput("Horizontal", TestInput.Axis.Horizontal, values.Count, selectedIndex);
                /*
                if (hasInput)
                {
                    input.OnUserInput += SettingValue_OnUserInput;
                }
                */
            }
            else
            {
                selectedIndex = ExtensionMethods.IncrementCircularInt(selectedIndex, values.Count, (int)Input.GetAxisRaw("Horizontal"));
            }

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                UpdateUserInterface();
            }
        }
    }

     private void UpdateUserInterface()
    {
        selectedValue = values[selectedIndex];
        if (type != Mode.Color)
        {
            UpdateText(selectedValue);
        }
        else
        {
            UpdateColor(selectedValue);
        }

        if (type != Mode.Carousel)
        {
            UpdateSlider(selectedIndex);
        }

        if (!isDirty)
        {
            isDirty = true;
        }
    }

    private void UpdateText(string value)
    {
        if (valueText != null)
        {
            valueText.SetText(value);
        }
    }

    private void UpdateColor(string value)
    {
        Image valueImage = transform.Find("Value/Value").GetComponent<Image>();
        float H, S, V;
        Color.RGBToHSV(GameManager.GetAccentColor(), out _, out S, out V);
        H = float.Parse(value) / 100;
        Color color = Color.HSVToRGB(H, S, V);
        valueImage.color = color;
        GameManager.SetAccentColor(color);
    }

    private void UpdateSlider(int value)
    {
        Slider slider = transform.GetComponentInChildren<Slider>();
        float totalValues = (float)(values.Count);
        float targetValue = 1f - (float)value / (totalValues - 1);
        StartCoroutine(LerpSlider(slider, targetValue, 0.1f));
    }

    private void ResetValue()
    {
        selectedValue = defaultValue;
        selectedIndex = values.IndexOf(selectedValue);
        UpdateUserInterface();
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
        //UpdateUserInterface();
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

        if (type == Mode.Color)
        {
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = i.ToString();
            }
            float H;
            Color.RGBToHSV(GameManager.GetAccentColor(), out H, out _, out _);
            defaultValue = Mathf.RoundToInt((H * 100)).ToString();
        }

        if (string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(defaultValue))
        {
            selectedValue = defaultValue;
            UpdateText(selectedValue);
        }

        selectedIndex = values.FindIndex(value => value == selectedValue);
        if (type != Mode.Carousel)
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

        // Debug
        if (Input.GetButtonDown("Remove"))
        {
            ResetValue();
        }
    }

    #endregion
}
