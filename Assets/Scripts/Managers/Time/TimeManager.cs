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

    private const float hoursPerDay = 24f;
    private const float minutesPerHours = 60f;

    [Header("Setup")]
    [SerializeField] private TimeUserInterface userInterface;

    [Header("Settings")]
    [Range(1f, 2840f)] [SerializeField] private float realSecondsPerDay = 1420f; // 1420f - 86400f
    [SerializeField] private Format format;

    [Header("Values")]
    [ReadOnly] [SerializeField] private float hours;
    [ReadOnly] [SerializeField] private float minutes;

    private float day;

    #if DEBUG
        [Header("Debug")]
    #endif
        [RenameField("Pause")] [SerializeField] private bool isPaused;

    #endregion

    #region Enums

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

    public float GetHours()
    {
        return hours;
    }

    public (string hours, string minutes, string period) GetTimeText()
    {
        string hours = null;
        string minutes = null;
        string period = null;

        if (format == Format.TwentyFour)
        {
            hours = this.hours.ToString("00");
            minutes = this.minutes.ToString("00");
        }
        else
        {
            string time;
            string[] splitTime;
            time = DateTime.ParseExact($"{this.hours.ToString("00")}:{this.minutes.ToString("00")}", "hh:mm", null).ToString("hh:mm:tt");
            splitTime = time.Split(':');

            hours = splitTime[0];
            minutes = splitTime[1];
            period = splitTime[2].ToUpper();
        }

        return (hours, minutes, period);
    }

    #endregion

    #region Miscellaneous Methods

    private void IncrementDays()
    {
        day += Time.unscaledDeltaTime / realSecondsPerDay;
    }

    private void IncrementHours()
    {
        if (hours != Mathf.Floor((GetDaysNormalized() * hoursPerDay)))
        {
            DiurnalCycleManager.instance.UpdateCycle();
        }
        hours = Mathf.Floor((GetDaysNormalized() * hoursPerDay));
    }

    private void IncrementMinutes()
    {
        minutes = Mathf.Floor(((GetDaysNormalized() * hoursPerDay % 1f)) * minutesPerHours);
    }

    private void IncrementTime()
    {
        IncrementDays();
        IncrementHours();
        IncrementMinutes();
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
        if (!isPaused)
        {
            IncrementTime();
        }

        // TODO: Debug
        if (PauseManager.instance.flags.isActive)
        {
            isPaused = true;
            (string hours, string minutes, string period) = GetTimeText();
            userInterface.SetTimeText(hours, minutes, period);
        }
    }

    #endregion
}