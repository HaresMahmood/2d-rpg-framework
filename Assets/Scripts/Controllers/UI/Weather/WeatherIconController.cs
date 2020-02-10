using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class WeatherIconController : MonoBehaviour
{
    #region Miscellaneous Methods

    public void SetWeatherAnimation(WeatherManager.WeatherState currentWeather, WeatherManager.WeatherState previousWeather)
    {
        transform.Find(previousWeather.ToString()).gameObject.SetActive(false);
        transform.Find(currentWeather.ToString()).gameObject.SetActive(true);
    }

    #endregion
}
