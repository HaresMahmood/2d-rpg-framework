using UnityEngine;
using TMPro;
using System;

/// <summary>
///
/// </summary>
public class TimeUserInterface : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI clockText;

    #endregion

    #region Helper Methods

    public void SetTimeText(string time)
    {
        clockText.SetText($"{time}");
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        clockText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    #endregion
}
