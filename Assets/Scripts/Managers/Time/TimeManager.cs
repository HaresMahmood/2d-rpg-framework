using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public class TimeManager : MonoBehaviour
{
    #region Variables

    public static TimeManager instance;

    private const float hoursPerDay = 24f;
    private const float minutesPerHours = 60f;

    [Header("Settings")]
    [Range(1f, 2840f)] [SerializeField] private float realSecondsPerDay = 1420f; // 1420f - 86400f
    [SerializeField] private Format format;

    [Header("Values")]
    [ReadOnly] [SerializeField] private float hours;
    [ReadOnly] [SerializeField] private float minutes;

    private float day;

    #if DEBUG
        [Header("Debug")]
        [RenameField("Pause")] [SerializeField] private bool isPaused;
    #endif

    private bool isDirty;

    private enum Period
    {
        AM,
        PM
    }

    private enum Format
    {
        TwentyFour,
        Twelve
    }

    #endregion

    #region Accessor Methods

    public float GetSecondsPerDay()
    {
        return realSecondsPerDay;
    }

    public float GetHoursPerDay()
    {
        return hoursPerDay;
    }

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

    public float GetHours()
    {
        return hours;
    }

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
#if DEBUG
        if (!isPaused)
        {
#endif
            day += Time.unscaledDeltaTime / realSecondsPerDay;
            hours = Mathf.Floor((GetDaysNormalized() * hoursPerDay));
            minutes = Mathf.Floor(((GetDaysNormalized() * hoursPerDay % 1f)) * minutesPerHours);
#if DEBUG
        }
#endif
    }

    #endregion
}
