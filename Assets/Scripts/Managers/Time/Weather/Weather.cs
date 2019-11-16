using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class Weather
{
    [SerializeField] private State weather;
    [SerializeField] private Sprite icon;

    public enum State
    {
        Clear,
        Cloudy,
        Rainy, 
        Stormy,
        Thunder,
        Snowy,
        Hailing,
        //Windy,
        Foggy
    }

    #region Accessor Methods

    public State GetState()
    {
        return weather;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    #endregion
}