using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class DiurnalCycleManager : MonoBehaviour
{
    #region Variables

    public static DiurnalCycleManager instance;

    private const int sunriseStart = 5;
    private const int sunriseEnd = 7;
    private const int noonStart = 12;
    private const int sunsetStart = 18;
    private const int sunsetEnd = 20;
    private const int midnightStart = 0;

    [UnityEngine.Header("Setup")]
    [SerializeField] private Light globalLight;

    [UnityEngine.Header("Settings")]
    [SerializeField] private Color day = "FFEAC9".ToColor();
    [SerializeField] private Color night = "546BAB".ToColor();
    [SerializeField] private Color twilight = "B273A2".ToColor();
    [SerializeField] private Color noon = "FCFFB5".ToColor();
    [SerializeField] private Color midnight = "001E3E".ToColor();

    private Color lightColor;
    private float lightIntensity;

    private float singleHour;

    #endregion

    #region Miscellaneous Methods

    private void UpdateCycle()
    {
        float duration;
        int time = (int)TimeManager.instance.GetHours();

        switch (time)
        {
            default: { break; }
            case (sunriseStart):
                {
                    duration = (sunriseEnd - sunriseStart - 1) * singleHour;
                    StartCoroutine(globalLight.gameObject.FadeColor(twilight, duration));
                    break;
                }
            case (sunriseEnd):
                {
                    duration = (noonStart - sunriseEnd - 1) * singleHour;
                    StartCoroutine(globalLight.gameObject.FadeColor(noon, duration));
                    break;
                }
            case (noonStart):
                {
                    duration = (sunsetStart - noonStart - 1) * singleHour;
                    StartCoroutine(globalLight.gameObject.FadeColor(day, duration));
                    break;
                }
            case (sunsetStart):
                {
                    duration = (sunsetEnd - sunsetStart - 1) * singleHour;
                    StartCoroutine(globalLight.gameObject.FadeColor(twilight, duration));
                    break;
                }
            case (sunsetEnd):
                {
                    duration = (midnightStart - sunriseEnd - 1) * singleHour;
                    StartCoroutine(globalLight.gameObject.FadeColor(midnight, duration));
                    break;
                }
            case (midnightStart):
                {
                    duration = (sunriseStart - midnightStart - 1) * singleHour;
                    StartCoroutine(globalLight.gameObject.FadeColor(night, duration));
                    break;
                }
        }
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

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        UpdateCycle();
    }

    #endregion
}
