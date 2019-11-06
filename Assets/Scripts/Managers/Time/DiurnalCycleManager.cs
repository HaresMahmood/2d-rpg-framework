using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class DiurnalCycleManager : MonoBehaviour
{
    #region Variables

    public static DiurnalCycleManager instance;

    private const int sunriseStart = 5;
    private const int sunriseEnd = 6;
    private const int noonStart = 12;
    private const int sunsetStart = 18;
    private const int sunsetEnd = 19;
    private const int midnightStart = 0;

    private const string twilight = "B273A2";
    private const string noon = "FCFFB5";
    private const string midnight = "001E3E";

    [UnityEngine.Header("Setup")]
    [SerializeField] private Light globalLight;

    private Color lightColor;
    private float lightIntensity;

    #endregion

    #region Helper Methods

    private void UpdateCycle()
    {
        int time = (int)TimeManager.instance.GetHours();
        float duration = TimeManager.instance.GetSecondsPerDay() / TimeManager.instance.GetHoursPerDay();
        switch (time)
        {
            default: { break; }
            case (sunriseStart):
                {
                    StartCoroutine(globalLight.gameObject.FadeColor(twilight.ToColor(), duration));
                    break;
                }
        }
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
        lightColor = globalLight.color;
        lightIntensity = globalLight.intensity;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        UpdateCycle();
    }

    #endregion
}
