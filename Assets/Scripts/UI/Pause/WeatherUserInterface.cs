using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class WeatherUserInterface : MonoBehaviour
{
    #region Variables

    private Transform weatherStateContainers;
    private TextMeshProUGUI weatherText;

    #endregion

    #region Helper Methods

    private void SetWeatherText(WeatherManager.WeatherState weather)
    {
        weatherText.SetText(weather.ToString());
    }

    private void SetWeatherAnimation(WeatherManager.WeatherState currentWeather, WeatherManager.WeatherState previousWeather)
    {
        weatherStateContainers.Find(previousWeather.ToString()).gameObject.SetActive(false);
        weatherStateContainers.Find(currentWeather.ToString()).gameObject.SetActive(true);
    }

    public void SetWeatherUserInterface(WeatherManager.WeatherState weather, WeatherManager.WeatherState previousWeather)
    {
        SetWeatherText(weather);
        SetWeatherAnimation(weather, previousWeather);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        weatherStateContainers = transform.Find("Weather States");
        weatherText = GetComponentInChildren<TextMeshProUGUI>();
    }

    #endregion
}
