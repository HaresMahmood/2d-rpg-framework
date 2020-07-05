using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class SettingUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => GetComponent<SettingUserInterfaceController>().Setting.Values.Count;

    #endregion

    #region Variables

    private SettingUserInterfaceController controller;

    private Slider slider;
    private TextMeshProUGUI valueText;

    #endregion

    #region Miscellaneous Methods

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        string value = controller.Setting.Values[selectedValue];

        if (controller.Setting.Type == Setting.SettingType.Slider)
        {
            UpdateSlider(int.Parse(value));
        }

        valueText.SetText(value);
        controller.Setting.Value = value;
    }

    /*
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
    */

    private void UpdateSlider(int selectedValue, float animationDuration = 0.1f)
    {
        float totalValues = (float)(controller.Setting.Values.Count);
        float targetValue = 1f - (float)selectedValue / (totalValues);

        slider.value = targetValue;

        //StartCoroutine(slider.LerpSlider(targetValue, animationDuration));
    }

    private int ResetValue()
    {
        int selectedValue = controller.Setting.Values.IndexOf(controller.Setting.DefaultValue);

        controller.Setting.Value = controller.Setting.DefaultValue;
        UpdateSelectedObject(selectedValue);

        return selectedValue;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        controller = GetComponent<SettingUserInterfaceController>();

        if (controller.Setting.Type == Setting.SettingType.Slider)
        {
            slider = transform.Find("Value/Slider").GetComponent<Slider>();
        }

        valueText = transform.Find("Value/Value").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateSelectedObject(controller.Setting.Values.IndexOf(controller.Setting.Value));
    }

    #endregion
}

