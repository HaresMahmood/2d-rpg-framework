using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokémon", menuName = "Characters/Pokémon")]
public class Pokemon : ScriptableObject
{
    public new string name;
    public int id;
    public int level;
    [Range(0f, 200f)] public float exp;
    public float totalExp = 200;
    public string category;
    public string dexEntry;
    public string ability;
    public string nature; // TODO: Should be enum.
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite menuSprite;
    public Type primaryType;
    public Type secondaryType;
    public List<LearnedMove> learnedMoves = new List<LearnedMove>();
    public Status status;
    public Item heldItem;

    [System.Serializable]
    public class LearnedMove
    {
        public Move move;
        public int remainingPp;
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