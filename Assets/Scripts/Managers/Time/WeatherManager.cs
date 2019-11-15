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

    private bool isFlashingLight;

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

    private void EnableParticleSystem(string parentSystem, string subSystem, int particlesRate, bool settings = false, float startLifetime = -1f, float gravity = -1,  Vector3? transformScale = null)
    {
        Transform target = particleSystems.Find(parentSystem).Find(subSystem);
        StartCoroutine(target.gameObject.FadeParticleSystem(particlesRate, 0.1f, true));

        if (settings)
        {
            ParticleSystem particleSystem = particleSystems.Find(parentSystem).Find(subSystem).GetComponent<ParticleSystem>();
            var main = particleSystem.main;

            if (transformScale != null)
            {
                target.localScale = (Vector3)transformScale;
            }

            if (startLifetime > -1)
            {
                main.startLifetime = startLifetime;
            }

            if (gravity > -1)
            {
                main.gravityModifier = gravity;
            }
        }
    }

    private void DisableParticleSystem(string particleSystem)
    {
        Transform target = particleSystems.Find(particleSystem);
        StartCoroutine(target.gameObject.FadeParticleSystem(0, 0.1f, true));
    }

    private void DisableAllWeatherSystems()
    {
        ParticleSystem[] targets = particleSystems.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem target in targets)
        {
            if (target.emission.rateOverTime.constant > 0)
            {
                StartCoroutine(target.gameObject.FadeParticleSystem(0, 0.1f, true));
            }
        }

        DisableWind();
        DisableFlashLight();
    }

    private void EnableWind(float windSpeed)
    {
        WindZone windZone = particleSystems.Find("Wind").GetComponent<WindZone>();

        windZone.gameObject.SetActive(true);
        windZone.windMain = windSpeed;
    }

    private void DisableWind()
    {
        WindZone windZone = particleSystems.Find("Wind").GetComponent<WindZone>();

        if (windZone.gameObject.activeSelf)
        {
            windZone.gameObject.SetActive(false);
        }
    }

    private IEnumerator EnableFlashLight()
    {
        isFlashingLight = true;

        while (isFlashingLight)
        {
            GlobalLightController lightController = DiurnalCycleManager.instance.GetGlobalLight().GetComponent<GlobalLightController>();
            int repetitions = Random.Range(2, 5);
            float interval = Random.Range(1f, 15f);

            StartCoroutine(lightController.FlashLight(3f, 0.6f, repetitions));
            yield return new WaitForSeconds(interval);
        }
    }
    
    private void DisableFlashLight()
    {
        if (isFlashingLight)
        {
            isFlashingLight = false;
        }
    }

    private void SetWeatherEffects(Weather weather)
    {
        switch (weather.GetState())
        {
            default: { break; }
            case (Weather.State.Clear):
                {
                    Color[] colors = new Color[] { "FFEAC9".ToColor(), "546BAB".ToColor(), "B273A2".ToColor(), "FCFFB5".ToColor(), "001E3E".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();                   
                    break;

                }
            case (Weather.State.Cloudy):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();
                    break;
                }
            case (Weather.State.Rainy):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();
                    EnableParticleSystem("Rain", "Rain Dropplets", 100, true, 0.7f, 5f, new Vector3(0.5f, 3.75f, 1f));
                    EnableParticleSystem("Rain", "Rain Splatter", 100);
                    break;
                }
            case (Weather.State.Stormy):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();
                    EnableParticleSystem("Rain", "Rain Dropplets", 200, true, 0.3f, 10f, new Vector3(0.5f, 10f, 1f));
                    EnableParticleSystem("Rain", "Rain Splatter", 200);
                    EnableWind(7.5f);
                    StartCoroutine(EnableFlashLight());
                    break;
                }
        }
    }

    public IEnumerator ChangeWeather()
    {
        List<Weather> weatherStates = new List<Weather>();
        //yield return new WaitUntil(() => SceneStreamManager.IsSceneLoaded(SceneStreamManager.instance.GetActiveScene()));
        while (!SceneStreamManager.IsSceneLoaded(SceneStreamManager.instance.GetActiveScene()))
        {
            yield return null;
        }
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
        SetWeatherEffects(weather);
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
