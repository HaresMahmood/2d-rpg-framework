using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class WeatherStates : MonoBehaviour
{
    #region Variables

    [SerializeField] private List<Weather> weatherStates = new List<Weather>();

    #endregion

    #region Accessor Methods

    public List<Weather> GetWeatherStates()
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
        //weatherStates = new List<WeatherManager.Weather>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
