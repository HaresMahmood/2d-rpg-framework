using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Moves/Move")]
public class Move : ScriptableObject
{
    public new string name;
    public int id;
    public int pp;
    public int remaindingPP;
    public Category category;
    public int accuracy;
    public int power;
    public string description;
    public Type type;
    public Color UIColor;

    public enum Category
    {
        None,
        Special,
        Physical
    }

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
        Fairy
    }
}