using UnityEngine;
using TMPro;
using System;

/// <summary>
///
/// </summary>
public class TimeUserInterface : MonoBehaviour
{
    #region Variables

    public static TimeUserInterface instance;

    private TextMeshProUGUI clockText;

    #endregion

    #region Helper Methods

    public void SetTimeText()
    {
        string time = TimeManager.instance.GetTimeText();
        clockText.SetText($"{time}");
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
        clockText = transform.Find("Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
