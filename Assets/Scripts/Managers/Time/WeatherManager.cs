using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class WeatherManager : MonoBehaviour
{
    #region Variables

    public static WeatherManager instance;

    [ReadOnly] [SerializeField] private Weather weather;
    [ReadOnly] [SerializeField] private Weather nextWeather;

    public enum Weather
    { 
        Sunny,
        Cloudy,
        Rainy,
        Snowy,
        Stormy,
        Thunder,
        Hailing,
        Windy,
        Foggy
    }

    #endregion

    #region Accessor Methods
    
    public (Weather weather, Weather nextWeather)  GetWeather()
    {
        return (weather: this.weather, nextWeather: this.nextWeather);
    }

    #endregion

    #region Helper Methods

    private Weather SetRandomWeather(List<Weather> weatherStates)
    {
        System.Random random = new System.Random();
        int randomState = random.Next(weatherStates.Count);
        return weatherStates[randomState];
    }

    #endregion

    #region Miscellaneous Methods

    public void ChangeWeather()
    {
        //GameManager.instance.transform.GetComponentInChildren<DiurnalCycleManager>().OnCycleUpdate += WeatherManager_OnCycleUpdate;
        List<Weather> weatherStates = new List<Weather>();
        if (SceneStreamManager.IsSceneLoaded(SceneStreamManager.instance.GetActiveScene()))
        {
            weatherStates = FindObjectOfType<WeatherStates>().GetWeatherStates();
            weather = nextWeather;
            nextWeather = SetRandomWeather(weatherStates);
        }
    }

    #endregion

    #region Event Methods

    private void WeatherManager_OnCycleUpdate(object sender, EventArgs e)
    {
        Debug.Log("EVENT METHOD CALLED");
        GameManager.instance.transform.GetComponentInChildren<DiurnalCycleManager>().OnCycleUpdate -= WeatherManager_OnCycleUpdate;
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
        
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
