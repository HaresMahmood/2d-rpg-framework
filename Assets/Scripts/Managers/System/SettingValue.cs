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

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        if (string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(defaultValue))
        {
            selectedValue = defaultValue;
            TextMeshProUGUI valueText = transform.Find("Value").GetComponentInChildren<TextMeshProUGUI>();
            valueText.SetText(selectedValue);
        }
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
