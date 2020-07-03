using UnityEngine;

/// <summary>
///
/// </summary>
public class SettingUserInterfaceController : UserInterfaceController
{
    #region Fields

    //private static SettingUserInterfaceController instance;

    //[SerializeField] private SettingUserInterface userInterface;
    [SerializeField] private Setting setting;

    #endregion

    #region Properties

    /*
    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static SettingUserInterfaceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SettingUserInterfaceController>();
            }

            return instance;
        }
    }
     */

    public Setting Setting
    {
        get { return setting; }
    }

    #endregion

    #region Miscellaneous Methods

    /*
    private void GetInput()
    {
        if (SystemManager.instance.flags.isActive && isSelected)
        {
            if (type != Type.Color)
            {
                (selectedIndex, _) = input.GetInput("Horizontal", TestInput.Axis.Horizontal, values.Count, selectedIndex, type == Type.Slider);
            }
            else
            {
                selectedIndex = ExtensionMethods.IncrementInt(selectedIndex, 0, values.Count, (int)Input.GetAxisRaw("Horizontal"), true);
            }

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                UpdateUserInterface();
            }
        }
    }
    */

    /*
     private void UpdateUserInterface()
    {
        selectedValue = values[selectedIndex];
        if (type != Type.Color)
        {
            UpdateText(selectedValue);
        }
        else
        {
            UpdateColor(selectedValue);
        }

        if (type != Type.Carousel)
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

        //StartCoroutine(slider.LerpSlider(targetValue, 0.15f));
    }

    private void ResetValue()
    {
        selectedValue = defaultValue;
        selectedIndex = values.IndexOf(selectedValue);
        UpdateUserInterface();
    }

    protected override void GetInput(string axis)
    {
        base.GetInput(axis);

        if (Input.GetButtonDown("Remove"))
        {
            // Reset value
        }
    }
    */

    #endregion

    #region Unity Methods

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected override void Update()
    {
        if (Flags.isActive)
        {
            GetInput("Horizontal");
        }
    }

    /*
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        valueText = transform.Find("Value").GetComponentInChildren<TextMeshProUGUI>();

        if (string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(defaultValue))
        {
            selectedValue = defaultValue;
            UpdateText(selectedValue);
        }

        selectedIndex = values.FindIndex(value => value == selectedValue);
        if (type != Type.Carousel)
        {
            UpdateSlider(selectedIndex);
        }
    }
    */

    #endregion
}
