using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokémon", menuName = "Characters/Pokémon")]
public class Pokemon : ScriptableObject
{
    #region Fields

    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] private int level;
    [SerializeField] private string category;
    [SerializeField] private string dexEntry;
    [SerializeField] private string nature;
    [SerializeField] private string ability;
    [SerializeField] private PokemonSprites sprites;
    [SerializeField] private Type primaryType;
    [SerializeField] private Type secondaryType;
    [SerializeField] private List<Move> activeMoves = new List<Move>();
    [SerializeField] private List<Move> learnedMoves = new List<Move>();
    [SerializeField] private StatusAilment status;
    [SerializeField] private Item heldItem;
    [SerializeField] private PokemonStats stats;

    #endregion

    #region Properties

    public string Name
    {
        get { return name; }
        private set { name = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public int Level
    {
        get { return level; }
        private set { level = value; }
    }

    public string Category
    {
        get { return category; }
        private set { category = value; }
    }

    public string DexEntry
    {
        get { return dexEntry; }
        private set { dexEntry = value; }
    }

    /*
    public int Nature
    {
        get { return level; }
        private set { level = value; }
    }

    public int Ability
    {
        get { return level; }
        private set { level = value; }
    }
    */

    public PokemonSprites Sprites
    {
        get { return sprites; }
        private set { sprites = value; }
    }

    public Type PrimaryType
    {
        get { return primaryType; }
        private set { primaryType = value; }
    }

    public Type SecondaryType
    {
        get { return secondaryType; }
        private set { secondaryType = value; }
    }

    public List<Move> ActiveMoves
    {
        get { return activeMoves; }
        private set { activeMoves = value; }
    }

    public List<Move> LearnedMoves
    {
        get { return learnedMoves; }
        private set { learnedMoves = value; }
    }

    public StatusAilment Status
    {
        get { return status; }
        private set { status = value; }
    }

    public Item HeldItem
    {
        get { return heldItem; }
        private set { heldItem = value; }
    }

    public PokemonStats Stats
    {
        get { return stats; }
        private set { stats = value; }
    }

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
        Fairy
    }

    public enum StatusAilment
    {
        None,
        Paralyzed,
        Burned,
        Frozen,
        Poisoned,
        Asleep
    }

    #endregion

    #region Nested Classes

    [System.Serializable]
    public class PokemonSprites
    {
        [SerializeField] private Sprite frontSprite;
        [SerializeField] private Sprite backSprite;
        [SerializeField] private Sprite menuSprite;

        public Sprite FrontSprite
        {
            get { return frontSprite; }
        }

        public Sprite BackSprite
        {
            get { return backSprite; }
        }

        public Sprite MenuSprite
        {
            get { return menuSprite; }
        }
    }

    [System.Serializable]
    public class PokemonStats
    {
        public int health;
        public int attack;
        public int defence;
        public int spAttack;
        public int spDefence;
        public int speed;

        private enum StatType
        {
            HP,
            Attack,
            Defence,
            SpAttack,
            SpDefence,
            Speed
        }

        public class Stat
        {
            public int stat;

            public Stat(int amount)
            {
                stat = amount;
            }
        }
    }

    #endregion
}