﻿using System.Collections;
using System.Collections.Generic;
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

    private enum TimeOfDay
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

    #region Helper Methods

    private void FadeLightColor(Color color, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(globalLight.gameObject.FadeColor(color, duration));
    }

    #endregion

    #region Miscellaneous Methods

    private void UpdateCycle()
    {
        int time = (int)TimeManager.instance.GetHours();

        switch (time)
        {
            default: { break; }
            case ((int)TimeOfDay.dawn):
                {
                    FadeLightColor(day, (3 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.morning):
                {
                    FadeLightColor(noon, (4 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.noon):
                {
                    FadeLightColor(day, (2* singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.afternoon):
                {
                    FadeLightColor(day, (4 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.dusk):
                {
                    FadeLightColor(twilight, (2 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.evening):
                {
                    FadeLightColor(night, (4 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.night):
                {
                    FadeLightColor(midnight, (2 * singleHour));
                    timeOfDay = (TimeOfDay)time;
                    break;
                }
            case ((int)TimeOfDay.midnight):
                {
                    FadeLightColor(twilight, (3 * singleHour));
                    timeOfDay = (TimeOfDay)time;
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
