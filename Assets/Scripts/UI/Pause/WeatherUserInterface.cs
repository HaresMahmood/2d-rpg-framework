using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class WeatherUserInterface : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI weatherText;

    private Transform[] weatherStateAnimations;

    #endregion

    #region Helper Methods

    private void SetWeatherText(WeatherManager.WeatherState weather)
    {
        weatherText.SetText(weather.ToString());
    }

    private void SetWeatherAnimation(WeatherManager.WeatherState weather)
    {
        weatherText.SetText(weather.ToString());
    }

    public void SetWeatherUserInterface(WeatherManager.WeatherState weather)
    {
        SetWeatherText(weather);
        SetWeatherAnimation(weather);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    { 
        weatherText = GetComponentInChildren<TextMeshProUGUI>();
        //weatherStateAnimations = 
    }

    #endregion
}
