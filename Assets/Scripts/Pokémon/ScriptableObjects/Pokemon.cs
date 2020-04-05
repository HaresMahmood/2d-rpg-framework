﻿using System.Collections.Generic;
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
    [SerializeField] private PokemonNature nature = new PokemonNature();
    [SerializeField] private string ability;
    [SerializeField] private PokemonSprites sprites;
    [SerializeField] private Type primaryType;
    [SerializeField] private Type secondaryType;
    [SerializeField] private List<Move> activeMoves = new List<Move>();
    [SerializeField] private List<Move> learnedMoves = new List<Move>();
    [SerializeField] private StatusAilment status = new StatusAilment();
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

    public PokemonNature Nature
    {
        get { return nature; }
    }

    /*
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

    #endregion

    #region Nested Classes

    [System.Serializable]
    public class PokemonNature
    {
        #region Fields

        [SerializeField] private Nature value;

        #endregion

        #region Properties

        public string Value
        {
            get { return value.ToString(); }
        }

        public (PokemonStats.StatType, PokemonStats.StatType) ChangedStats
        {
            get
            {
                PokemonStats.StatType increasedStat;
                PokemonStats.StatType decreasedStat;

                if (value == Nature.Lonely || value == Nature.Brave || value == Nature.Adamant || value == Nature.Naughty)
                {
                    increasedStat = PokemonStats.StatType.Attack;
                }
                else if (value == Nature.Bold || value == Nature.Relaxed || value == Nature.Impish || value == Nature.Lax)
                {
                    increasedStat = PokemonStats.StatType.Defence;
                }
                else if (value == Nature.Timid || value == Nature.Hasty || value == Nature.Jolly || value == Nature.Naive)
                {
                    increasedStat = PokemonStats.StatType.Speed;
                }
                else if (value == Nature.Modest || value == Nature.Mild || value == Nature.Quiet || value == Nature.Rash)
                {
                    increasedStat = PokemonStats.StatType.SpAttack;
                }
                else if (value == Nature.Calm || value == Nature.Gentle || value == Nature.Sassy || value == Nature.Careful)
                {
                    increasedStat = PokemonStats.StatType.SpDefence;
                }
                else
                {
                    increasedStat = PokemonStats.StatType.None;
                }

                if (value == Nature.Lonely || value == Nature.Docile || value == Nature.Mild || value == Nature.Gentle)
                {
                    decreasedStat = PokemonStats.StatType.Defence;
                }
                else if (value == Nature.Brave || value == Nature.Relaxed || value == Nature.Quiet || value == Nature.Sassy)
                {
                    decreasedStat = PokemonStats.StatType.Speed;
                }
                else if (value == Nature.Adamant || value == Nature.Impish || value == Nature.Jolly || value == Nature.Careful)
                {
                    decreasedStat = PokemonStats.StatType.SpAttack;
                }
                else if (value == Nature.Naughty || value == Nature.Lax || value == Nature.Naive || value == Nature.Rash)
                {
                    decreasedStat = PokemonStats.StatType.SpDefence;
                }
                else if (value == Nature.Bold || value == Nature.Timid || value == Nature.Modest || value == Nature.Calm)
                {
                    decreasedStat = PokemonStats.StatType.Attack;
                }
                else
                {
                    decreasedStat = PokemonStats.StatType.None;
                }

                return (increasedStat, decreasedStat);
            }
        }

        #endregion

        #region Enums

        public enum Nature
        {
            Hardy,
            Lonely,
            Brave,
            Adamant,
            Naughty,
            Bold,
            Docile,
            Relaxed,
            Impish,
            Lax,
            Timid,
            Hasty,
            Serious,
            Jolly,
            Naive,
            Modest,
            Mild,
            Quiet,
            Bashful,
            Rash,
            Calm,
            Gentle,
            Sassy,
            Careful,
            Quirky
        }

        #endregion
    }

    [System.Serializable]
    public class StatusAilment
    {
        #region Fields

        [SerializeField] private Ailment value;

        #endregion

        #region Properties

        public string Value
        {
            get { return value.ToString(); }
        }

        public Color Color
        {
            get
            {
                Color color;

                switch (value)
                {
                    case Ailment.Paralyzed:
                        {
                            color = "FCFF83".ToColor();
                            break;
                        }
                    case Ailment.Burned:
                        {
                            color = "FF9D83".ToColor();
                            break;
                        }
                    case Ailment.Frozen:
                        {
                            color = "A0FCFF".ToColor();
                            break;
                        }
                    case Ailment.Poisoned:
                        {
                            color = "F281FF".ToColor();
                            break;
                        }
                    case Ailment.Asleep:
                        {
                            color = "B0B0B0".ToColor();
                            break;
                        }
                    default:
                        {
                            color = Color.white;
                            break;
                        }
                }

                return color;
            }
        }

        #endregion

        #region Enums

        public enum Ailment
        {
            None,
            Paralyzed,
            Burned,
            Frozen,
            Poisoned,
            Asleep
        }

        #endregion
    }

    [System.Serializable]
    public class PokemonSprites
    {
        #region Fields

        [SerializeField] private Sprite frontSprite;
        [SerializeField] private Sprite backSprite;
        [SerializeField] private Sprite menuSprite;

        #endregion

        #region Properties

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

        #endregion
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

        public enum StatType
        {
            None,
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