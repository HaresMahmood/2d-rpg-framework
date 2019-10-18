using UnityEngine;

[CreateAssetMenu(fileName = "New Pokémon", menuName = "Characters/Pokémon")]
public class Pokemon : ScriptableObject
{
    public new string name;
    public int id;
    public string dexEntry;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite menuSprite;
    public Type primaryType;
    public Type secondaryType;
    public Item heldItem;

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