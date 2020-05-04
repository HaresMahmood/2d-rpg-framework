using UnityEngine;

[System.Serializable]
public class Typing
{
    #region Properties

    public Type Value { get; set; }
    public Color color { get; set; }

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