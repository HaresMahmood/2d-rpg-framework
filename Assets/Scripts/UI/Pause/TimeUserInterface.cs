using System;
using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class TimeUserInterface : TimeManager
{
    #region Variables

    private TextMeshProUGUI clockText;

    private Period period;

    private bool isDirty;

    private enum Period
    {
        AM,
        PM
    }

    #endregion

    #region Helper Methods

    private IEnumerator IncrementPeriod(Period currentPeriod)
    {
        int newPeriod = ExtensionMethods.IncrementCircularInt((int)currentPeriod, 2, 1);
        period = (Period)newPeriod;
        yield return new WaitForSecondsRealtime(2f);
        isDirty = false;
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
    protected override void Update()
    {
        (float hours, float minutes) = GetTime();

        // Debug
        if (GetFormat() == Format.Twelve)
        {
            if (!isDirty && (hours == 0 && minutes == 0))
            {
                isDirty = true;
                StartCoroutine(IncrementPeriod(period));
            }
            clockText.SetText($"{hours.ToString("00")}:{minutes.ToString("00")} <color=#696969>{period.ToString()}</color>");
        }
        else
        {
            clockText.SetText($"{hours}:{minutes}");
        }

        base.Update();
    }

    #endregion
}
