using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokémon", menuName = "Characters/Pokémon")]
public class Pokemon : ScriptableObject
{
    public new string name;
    public int id;
    public int level;
    public string category;
    public string dexEntry;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite menuSprite;
    public Type primaryType;
    public Type secondaryType;
    public List<Move> moves = new List<Move>();
    public Status status;
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

    public enum Status
    {
        None,
        Paralyzed,
        Burned,
        Frozen,
        Poisoned,
        Asleep
    }
}