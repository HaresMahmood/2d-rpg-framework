using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class WeatherManager : MonoBehaviour
{
    #region Variables

    public static WeatherManager instance;

    [Header("Values")]
    [SerializeField] private Transform particleSystems;

    [Header("Values")]
    [ReadOnly] [SerializeField] private Weather weather;
    [ReadOnly] [SerializeField] private Weather nextWeather;

    #endregion

    #region Accessor Methods

    /*
    public (Weather weather, Weather nextWeather)  GetWeather()
    {
        return (weather: this.weather, nextWeather: this.nextWeather);
    }
    */

    public Weather GetCurrentWeather()
    {
        return weather;
    }

    #endregion

    #region Helper Methods

    private Weather SetRandomWeather(List<Weather> weatherStates)
    {
        int randomState = Random.Range(0, weatherStates.Count);
        return weatherStates[randomState];
    }

    #endregion

    #region Miscellaneous Methods

    private void EnableParticleSystem(string particleSystem, float particles)
    {
        StartCoroutine(particleSystems.Find(particleSystem).gameObject.FadeParticleSystem(particles, 0.5f, true));
    }

    private void DisableParticleSystem(string particleSystem)
    {
        StartCoroutine(particleSystems.Find(particleSystem).gameObject.FadeParticleSystem(0f, 0.5f, true));
    }

    private void SetWeatherColors(Weather weather)
    {
        switch (weather.GetState())
        {
            default: { break;  }
            case (Weather.State.Clear):
                {
                    Color[] colors = new Color[] { "FFEAC9".ToColor(), "546BAB".ToColor(), "B273A2".ToColor(), "FCFFB5".ToColor(), "001E3E".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableParticleSystem("Rain");
                    break;
                }
            case (Weather.State.Cloudy):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableParticleSystem("Rain");
                    break;
                }
            case (Weather.State.Rainy):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    EnableParticleSystem("Rain", 100f);
                    break;
                }
        }
    }

    public IEnumerator ChangeWeather()
    {
        List<Weather> weatherStates = new List<Weather>();
        yield return new WaitUntil(() => SceneStreamManager.IsSceneLoaded(SceneStreamManager.instance.GetActiveScene()));
        weatherStates = FindObjectOfType<WeatherStates>().GetWeatherStates();
        if (weather.GetIcon() == null)
        {
            weather = SetRandomWeather(weatherStates);
        }
        else
        {
            weather = nextWeather;
        }
        nextWeather = SetRandomWeather(weatherStates);

        SetWeatherColors(weather);
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
        StartCoroutine(ChangeWeather());
    }

    #endregion
}
