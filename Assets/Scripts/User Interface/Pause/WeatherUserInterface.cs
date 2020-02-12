using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class WeatherUserInterface : MonoBehaviour
{
    #region Variables

    private WeatherIconController weatherIcons;
    private TextMeshProUGUI weatherText;

    #endregion

    #region Helper Methods

    private void SetWeatherText(WeatherManager.WeatherState weather)
    {
        weatherText.SetText(weather.ToString());
    }

    public void SetWeatherUserInterface(WeatherManager.WeatherState weather, WeatherManager.WeatherState previousWeather)
    {
        SetWeatherText(weather);
        weatherIcons.SetWeatherAnimation(weather, previousWeather);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        weatherIcons = transform.parent.FindSibling("Weather Icons").GetComponent<WeatherIconController>();
        weatherText = GetComponentInChildren<TextMeshProUGUI>();
    }

    #endregion
}
