using UnityEngine;
using TMPro;
using System;

/// <summary>
///
/// </summary>
public class TimeUserInterface : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI hoursText;
    private TextMeshProUGUI minutesText;
    private TextMeshProUGUI periodText;

    #endregion

    #region Helper Methods

    public void SetTimeText(string hours, string minutes, string period)
    {
        hoursText.SetText(hours);
        minutesText.SetText(minutes);
        if (period != null)
        {
            periodText.gameObject.SetActive(true);
            periodText.SetText(period);
        }
        else
        {
            periodText.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        hoursText = transform.Find("Hours").GetComponentInChildren<TextMeshProUGUI>();
        minutesText = transform.Find("Minutes").GetComponentInChildren<TextMeshProUGUI>();
        periodText = transform.Find("Period").GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    #endregion
}
