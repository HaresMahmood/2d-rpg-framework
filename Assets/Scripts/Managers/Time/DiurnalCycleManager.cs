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
    private const int sunriseEnd = 8;
    private const int noonStart = 12;
    private const int sunsetStart = 18;
    private const int sunsetEnd = 20;
    private const int midnightStart = 0;

    [Header("Setup")]
    [SerializeField] private Light globalLight;

    [Header("Settings")]
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
                    SetLightColor(twilight, (3 * singleHour));
                    break;
                }
            case (sunriseEnd):
                {
                    SetLightColor(noon, (5 * singleHour));
                    break;
                }
            case (noonStart):
                {
                    SetLightColor(day, (6 * singleHour));
                    break;
                }
            case (sunsetStart):
                {
                    SetLightColor(twilight, (2 * singleHour));
                    break;
                }
            case (sunsetEnd):
                {
                    SetLightColor(midnight, (4 * singleHour));
                    break;
                }
            case (midnightStart):
                {
                    SetLightColor(night, (5 * singleHour));
                    break;
                }
        }
    }

    private void SetLightColor(Color color, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(globalLight.gameObject.FadeColor(color, duration));
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
