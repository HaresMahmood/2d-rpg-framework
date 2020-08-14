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

    [Header("Setup")]
    [SerializeField] private WeatherUserInterface userInterface;
    [SerializeField] private Transform particleSystems;

    [Header("Values")]
    [ReadOnly] [SerializeField] private WeatherState previousWeather;
    [ReadOnly] [SerializeField] private WeatherState weather;
    [ReadOnly] [SerializeField] private WeatherState nextWeather;

    private bool isFlashingLight;

    #endregion

    #region Enums

    public enum WeatherState
    {
        None,
        Clear,
        Clouded,
        Rain,
        Storm,
        Thunder,
        Snow,
        Hail,
        Fog
    }

    #endregion

    #region Accessor Methods

    /*
    public (Weather weather, Weather nextWeather)  GetWeather()
    {
        return (weather: this.weather, nextWeather: this.nextWeather);
    }
    */

    public WeatherState GetCurrentWeather()
    {
        return weather;
    }

    #endregion

    #region Helper Methods

    private WeatherState SetRandomWeather(List<WeatherState> weatherStates)
    {
        int randomState = Random.Range(0, weatherStates.Count);

        return weatherStates[randomState];
    }

    #endregion

    #region Miscellaneous Methods

    private void EnableParticleSystem(string parentSystem, string subSystem, int particlesRate, float duration, bool settings = false, float startLifetime = -1f, float gravity = -1,  Vector3? transformScale = null)
    {
        Transform target = particleSystems.Find(parentSystem).Find(subSystem);
        StartCoroutine(target.gameObject.FadeParticleSystem(particlesRate, duration, true));

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
                StartCoroutine(target.gameObject.FadeParticleSystem(0, 0.001f, true));
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

    private void SetWeatherEffects(WeatherState weather)
    {
        switch (weather)
        {
            default: { break; }
            case (WeatherState.Clear):
                {
                    Color[] colors = new Color[] { "FFEAC9".ToColor(), "546BAB".ToColor(), "B273A2".ToColor(), "FCFFB5".ToColor(), "001E3E".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();                   
                    break;

                }
            case (WeatherState.Clouded):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();
                    break;
                }
            case (WeatherState.Rain):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();
                    EnableParticleSystem("Rain", "Rain Dropplets", 100, 0.1f, true, 0.7f, 5f, new Vector3(0.5f, 3.75f, 1f));
                    EnableParticleSystem("Rain", "Rain Splatter (Sprites)", 50, 0.1f, true, -1f, -1f, new Vector3(0.5f, 2.5f, 1f));
                    EnableParticleSystem("Rain", "Rain Splatter (Ground)", 100, 0.1f);
                    break;
                }
            case (WeatherState.Storm):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();
                    EnableParticleSystem("Rain", "Rain Dropplets", 200, 0.1f, true, 0.3f, 10f, new Vector3(0.5f, 7.5f, 1f));
                    EnableParticleSystem("Rain", "Rain Splatter (Sprites)", 100, 0.1f, true, -1f, -1f, new Vector3(0.5f, 3.5f, 1f));
                    EnableParticleSystem("Rain", "Rain Splatter (Ground)", 200, 0.1f);
                    EnableWind(7.5f);
                    break;
                }
            case (WeatherState.Thunder):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();
                    EnableParticleSystem("Rain", "Rain Dropplets", 200, 0.1f, true, 0.3f, 10f, new Vector3(0.5f, 7.5f, 1f));
                    EnableParticleSystem("Rain", "Rain Splatter (Sprites)", 100, 0.1f, true, -1f, -1f, new Vector3(0.5f, 3.5f, 1f));
                    EnableParticleSystem("Rain", "Rain Splatter (Ground)", 200, 0.1f);
                    EnableWind(7.5f);
                    StartCoroutine(EnableFlashLight());
                    break;
                }
            case (WeatherState.Snow):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();
                    EnableParticleSystem("Snow", "Snow Flakes (Front)", 50, 0.01f);
                    EnableParticleSystem("Snow", "Snow Flakes (Back)", 50, 0.01f);
                    EnableParticleSystem("Snow", "Snow Flakes (Ground)", 35, 0.2f);
                    EnableWind(0.5f);
                    break;
                }
            case (WeatherState.Hail):
                {
                    Color[] colors = new Color[] { "8B959A".ToColor(), "4E5E8C".ToColor(), "34617E".ToColor(), "A1A29B".ToColor(), "0A1E33".ToColor() };
                    DiurnalCycleManager.instance.SetColors(colors);
                    DisableAllWeatherSystems();
                    EnableParticleSystem("Hail", "Hail Stones", 20, 0.01f);
                    EnableParticleSystem("Hail", "Hail Clatter (Ground)", 25, 0.01f);
                    EnableWind(0.5f);
                    break;
                }
        }
    }

    public void UpdateWeahterUserInterface()
    {
        userInterface.SetWeatherUserInterface(weather, previousWeather);
    }

    public IEnumerator ChangeWeather()
    {
        //yield return new WaitUntil(() => SceneStreamManager.IsSceneLoaded(SceneStreamManager.instance.GetActiveScene()));
        //while (!SceneStreamManager.IsSceneLoaded(SceneStreamManager.instance.GetActiveScene()))
        //{
            yield return null;
        //}

        List<WeatherState> weatherStates = FindObjectOfType<WeatherStates>().GetWeatherStates();

        if (weather == WeatherState.None)
        {
            weather = SetRandomWeather(weatherStates);
            nextWeather = SetRandomWeather(weatherStates);
        }
        else
        {
            previousWeather = weather;
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
