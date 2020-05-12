using UnityEngine;

[System.Serializable]
public class Typing
{
    #region Fields

    [SerializeField] private Type value;

    #endregion

    #region Properties

    public Type Value 
    {
        get { return value; }
        set { this.value = value; } 
    }

    public Color Color { get; set; }

    #endregion

    #region Enums

    public enum Type
    {
        None,
        Normal,
        Fire,
        Water,
        Grass,
        Electric,
        Fighting,
        Flying,
        Poison,
        Ground,
        Rock,
        Psychic,
        Bug,
        Ghost,
        Steel,
        Dark,
        Dragon,
        Fairy,
        Ice
    }

    #endregion
}