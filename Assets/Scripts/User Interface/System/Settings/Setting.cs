using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

[CreateAssetMenu(fileName = "New Setting", menuName = "Setting")]
public class Setting : ScriptableObject
{
    #region Fields

    [SerializeField] private SettingType type;
    [SerializeField] private List<string> values = new List<string>();
    [SerializeField] private string value;
    [SerializeField] private string defaultValue;
    [SerializeField] [TextArea] private string description;  

    #endregion

    #region Properties

    public SettingType Type
    {
        get { return type; }
        set { type = value; }
    }
    public List<string> Values
    {
        get { return values; }
    }

    public string DefaultValue
    {
        get { if (value == "") { defaultValue = values[0]; } return defaultValue; }
        set { defaultValue = value; }
    }

    public string Value
    {
        get { if (value == "") { value = defaultValue; } return value; }
        set { this.value = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    #endregion

    #region Enums

    public enum SettingType
    {
        Slider,
        Carousel
    }

    #endregion
}

