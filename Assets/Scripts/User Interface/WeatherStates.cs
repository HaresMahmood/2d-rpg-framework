using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class WeatherStates : MonoBehaviour
{
    #region Variables

    [SerializeField] List<WeatherManager.WeatherState> weatherStates = new List<WeatherManager.WeatherState>();

    #endregion

    #region Accessor Methods

    public List<WeatherManager.WeatherState> GetWeatherStates()
    {
        return weatherStates;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
