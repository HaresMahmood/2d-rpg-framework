using System;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class ClockUserInterface : MonoBehaviour
{
    #region Variables

    private const float realSecondsPerDays = 120f; //8640f
    private const float minutesPerHours = 60f;

    [SerializeField] private Format format;

    private TextMeshProUGUI clockText;

    private Period period;
    private float day;

    private enum Format
    {
        Twentyfour = 24,
        Twelve = 12
    }

    private enum Period
    {
        AM,
        PM
    }

    #endregion

    #region Helper Methods

    private void IncrementPeriod(Period currentPeriod)
    {
        int newPeriod = ExtensionMethods.IncrementCircularInt((int)currentPeriod, 2, 1);
        period = (Period)newPeriod;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        clockText = transform.Find("Value").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        float dayNormalized = day % 1f;
        string hours = Mathf.Floor((dayNormalized * (int)format)).ToString("00");
        string minutes = Mathf.Floor((((dayNormalized * (int)format) % 1f)) * minutesPerHours).ToString("00");

        day += Time.unscaledDeltaTime / realSecondsPerDays;
        //clockText.SetText($"{hours}:{minutes} <color=#696969>{period.ToString()}</color>");

        if ((int.Parse(hours) == 0 && int.Parse(minutes) == 1) && format == Format.Twelve)
        {
            IncrementPeriod(period);
        }

        // Debug
        if (format == Format.Twelve)
        {
            clockText.SetText($"{hours}:{minutes} <color=#696969>{period.ToString()}</color>");
        }
        else
        {
            clockText.SetText($"{hours}:{minutes}");
        }
    }

    #endregion
}
