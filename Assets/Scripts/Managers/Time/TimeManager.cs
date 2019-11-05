using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class TimeManager : MonoBehaviour
{
    #region Variables

    public const float realSecondsPerDays = 1420f; //1420f
    public const float hoursPerDay = 24f;
    public const float minutesPerHours = 60f;

    [SerializeField] private Format format;

    private float day;
    private float hours;
    private float minutes;

    public enum Format
    {
        Twentyfour = 24,
        Twelve = 12
    }

    #region Accessor Methods

    public float GetDays()
    {
        return day;
    }

    public float GetDaysNormalized()
    {
        return day % 1f;
    }

    public (float hours, float minutes) GetTime()
    {
        return (hours: hours, minutes: minutes);
    }

    public Format GetFormat()
    {
        return format;
    }

    #endregion

    #region Helper Methods

    public void ConvertTime()
    {
        switch (format)
        {
            default: { break; };
            case Format.Twelve:
                {
                    hours = format.ToString() == "AM" ? hours : (hours % (hoursPerDay / 2)) + (hoursPerDay / 2);
                    break;
                }
            case Format.Twentyfour:
                {
                    // TODO: Not correct conversion!
                    hours = Mathf.Clamp(hours, 0, (hoursPerDay / 2));
                    hours = Mathf.Floor(hours /= 2);
                    break;
                }
        }
    }

    #endregion

    #endregion

    #region Unity Methods

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    protected virtual void Update()
    {
        Format currentFormat = GetFormat();

        day += Time.unscaledDeltaTime / realSecondsPerDays;

        if (currentFormat != format)
        {
            currentFormat = format;
            ConvertTime();
        }

        hours = Mathf.Floor((GetDaysNormalized() * (int)format));
        minutes = Mathf.Floor((((GetDaysNormalized() * (int)format) % 1f)) * minutesPerHours);
    }

    #endregion
}
