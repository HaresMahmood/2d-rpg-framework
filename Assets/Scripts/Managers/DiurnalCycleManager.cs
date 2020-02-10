using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public class DiurnalCycleManager : MonoBehaviour
{
    #region Variables

    public static DiurnalCycleManager instance;

    [Header("Setup")]
    [SerializeField] private Light globalLight;

    [Header("Settings")]
    [SerializeField] private Color day = "FFEAC9".ToColor();
    [SerializeField] private Color night = "546BAB".ToColor();
    [SerializeField] private Color twilight = "B273A2".ToColor();
    [SerializeField] private Color noon = "FCFFB5".ToColor();
    [SerializeField] private Color midnight = "001E3E".ToColor();

    [Header("Values")]
    [ReadOnly] [SerializeField] private TimeOfDay timeOfDay;

    private Color lightColor;
    private float lightIntensity;

    private float singleHour;

    public event EventHandler OnCycleUpdate = delegate { };

    public enum TimeOfDay
    {
        dawn = 5,
        morning = 8,
        noon = 12,
        afternoon = 14,
        dusk = 18,
        evening = 20,
        night = 0,
        midnight = 2
    }

    #endregion

    #region Accessor Methods

    public Light GetGlobalLight()
    {
        return globalLight;
    }

    public TimeOfDay GetTimeOfDay()
    {
        return timeOfDay;
    }

    #endregion

    #region Helper Methods

    public void SetColors(Color[] colors)
    {
        Color currentColor = lightColor;

        day = colors[0];
        night = colors[1];
        twilight = colors[2];
        noon = colors[3];
        midnight = colors[4];
    }

    #endregion

    #region Miscellaneous Methods

    public void UpdateCycle()
    {
        int time = (int)TimeManager.instance.GetHours();

        OnCycleUpdate?.Invoke(this, EventArgs.Empty);

        switch (time)
        {
            default: { break; }
            case ((int)TimeOfDay.dawn):
                {
                    FadeLight(day, (3 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.morning):
                {
                    StartCoroutine(WeatherManager.instance.ChangeWeather());
                    FadeLight(noon, (4 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.noon):
                {
                    FadeLight(day, (2* singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.afternoon):
                {
                    StartCoroutine(WeatherManager.instance.ChangeWeather());
                    FadeLight(day, (4 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.dusk):
                {
                    StartCoroutine(WeatherManager.instance.ChangeWeather()); // Debug?
                    FadeLight(twilight, (2 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.evening):
                {
                    FadeLight(night, (4 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.night):
                {
                    StartCoroutine(WeatherManager.instance.ChangeWeather());
                    FadeLight(midnight, (2 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.midnight):
                {
                    FadeLight(twilight, (3 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
        }
    }

    private void FadeLight(Color color, float duration, float intensity = -1)
    {
        globalLight.GetComponent<GlobalLightController>().FadeLight(color, duration, intensity);
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
        lightColor = globalLight.color;
        lightIntensity = globalLight.intensity;

        singleHour = TimeManager.instance.GetSecondsPerDay() / TimeManager.instance.GetHoursPerDay();
    }

    #endregion
}
