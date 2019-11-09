using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class WeatherUserInterface : MonoBehaviour
{
    #region Variables

    public static WeatherUserInterface instance;

    private TextMeshProUGUI weatherText;
    private Image weatherIcon;

    #endregion

    #region Helper Methods

    public void SetWeatherUserInterface()
    {
        Weather weather = WeatherManager.instance.GetCurrentWeather();
        string text = weather.GetState().ToString();
        Sprite icon = weather.GetIcon();
        weatherText.SetText($"{text}");
        weatherIcon.sprite = icon;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        weatherText = GetComponentInChildren<TextMeshProUGUI>();
        weatherIcon = GetComponentInChildren<Image>();
    }

    #endregion
}
