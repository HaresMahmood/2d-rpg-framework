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

    private void SetTimeText()
    {
        string time = TimeManager.instance.GetTime();
        clockText.SetText($"{time}");
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
        if (PauseManager.instance.isPaused)
        {
            SetTimeText();
        }
    }

    #endregion
}
