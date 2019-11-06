using System;
using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class TimeManager : MonoBehaviour
{
    #region Variables

    public static TimeManager instance;

    public const float realSecondsPerDays = 30f; //1420f
    public const float hoursPerDay = 24f;
    public const float minutesPerHours = 60f;

    [UnityEngine.Header("Settings")]
    [SerializeField] private Format format;

    private float day;
    [UnityEngine.Header("Values")]
    [ReadOnly] [SerializeField] private float hours;
    [ReadOnly] [SerializeField] private float minutes;

    private bool isDirty;

    public enum Period
    {
        AM,
        PM
    }

    public enum Format
    {
        TwentyFour,
        Twelve
    }

    #endregion

    #region Accessor Methods

    public float GetDays()
    {
        return day;
    }

    public float GetDaysNormalized()
    {
        return day % 1f;
    }

    /*
    public (float hours, float minutes) GetTime()
    {
        return (hours: hours, minutes: minutes);
    }
    */

    public string GetTime()
    {
        string time;
        if (format == Format.TwentyFour)
        {
            time = $"{hours.ToString("00")}:{minutes.ToString("00")}";
        }
        else
        {
            time = DateTime.ParseExact($"{hours.ToString("00")}:{minutes.ToString("00")}", "HH:mm", null).ToString("hh:mm tt");
            if (time.ToUpper().Contains(Period.AM.ToString()))
            {
                time = time.Replace(Period.AM.ToString(), $"<color=#696969>{Period.AM.ToString()}</color>");
            }
            else
            {
                time = time.Replace(Period.PM.ToString(), $"<color=#696969>{Period.PM.ToString()}</color>");
            }
        }

        return time;
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
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        day += Time.unscaledDeltaTime / realSecondsPerDays;
        hours = Mathf.Floor((GetDaysNormalized() * hoursPerDay));
        minutes = Mathf.Floor(((GetDaysNormalized() * hoursPerDay % 1f)) * minutesPerHours);
    }

    #endregion
}
