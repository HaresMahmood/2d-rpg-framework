using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokémon", menuName = "Characters/Pokémon")]
public class Pokemon : ScriptableObject
{
    #region Fields

    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] private PokemonExperience experience = new PokemonExperience();
    [SerializeField] private int metAt;
    [SerializeField] private int hp;
    [SerializeField] private string category;
    [SerializeField] private string dexEntry;
    [SerializeField] private PokemonNature nature = new PokemonNature();
    [SerializeField] private string ability;
    [SerializeField] private PokemonSprites sprites;
    [SerializeField] private Typing primaryType;
    [SerializeField] private Typing secondaryType;
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
        set { name = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public PokemonExperience Experience 
    {
        get { return experience; } 
    }

    public int MetAt
    {
        get { return metAt; }
        set { metAt = value; }
    }

    public int HP
    {
        get { return hp; }
        private set { hp = value; }
    }

    public string Category
    {
        get { return category; }
        set { category = value; }
    }

    public string DexEntry
    {
        get { return dexEntry; }
        set { dexEntry = value; }
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

    public Typing PrimaryType
    {
        get { return primaryType; }
        set { primaryType = value; }
    }

    public Typing SecondaryType
    {
        get { return secondaryType; }
        set { secondaryType = value; }
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
        set { heldItem = value; }
    }

    public PokemonStats Stats
    {
        get { return stats; }
        private set { stats = value; }
    }

    #endregion

    #region Enums

    public enum Typing
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

    public enum Stat
    {
        None,
        HP,
        Attack,
        Defence,
        SpAttack,
        SpDefence,
        Speed
    }

    #endregion

    #region Nested Classes

    [System.Serializable]
    public class PokemonExperience
    {
        #region Properties

        [SerializeField] private int value;
        [SerializeField] private int level;
        [SerializeField] ExperienceGroup group;

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public ExperienceGroup Group
        {
            get { return group; }
            set { group = value; }
        }

        public int Total
        {
            get
            {
                return Mathf.Max(0, GetTotalExperience(Level));
            }
        }

        public int Remaining
        {
            get
            {
                if (Level > 0 && Level < 100)
                {
                    return (GetTotalExperience(Level + 1) - Total);
                }

                return 0;
            }
        }

        #endregion

        #region Enums

        public enum ExperienceGroup
        {
            Erratic,
            Fast,
            MediumFast,
            MediumSlow,
            Slow,
            Fluctuating
        }

        #endregion

        #region Miscellaneous Methods

        private int GetTotalExperience(int level)
        {
            int maxExperience = 0;

            switch (Group)
            {
                default: { break; }
                case ExperienceGroup.Erratic:
                    {
                        if (level <= 50)
                        {
                            maxExperience = (int)Mathf.Pow(level, 3) * (100 - level) / 50;
                        }
                        else if (level > 50 && level <= 68)
                        {
                            maxExperience = (int)Mathf.Pow(level, 3) * (150 - level) / 100;
                        }
                        else if (level > 68 && level <= 98)
                        {
                            maxExperience = (int)Mathf.Pow(level, 3) * ((1911 - 10 * level) / 3) / 500;
                        }
                        else if (level > 98 && level <= 100)
                        {
                            maxExperience = (int)Mathf.Pow(level, 3) * (160 - level) / 100;
                        }

                        break;
                    }
                case ExperienceGroup.Fast:
                    {
                        maxExperience = 4 * (int)Mathf.Pow(level, 3) / 5;
                        break;
                    }
                case ExperienceGroup.MediumFast:
                    {
                        maxExperience = level ^ 3;
                        break;
                    }
                case ExperienceGroup.MediumSlow:
                    {
                        maxExperience = ((int)(1.2f * (int)Mathf.Pow(level, 3))) - (15 * (int)Mathf.Pow(level, 2)) + (100 * level) - 140;
                        break;
                    }
                case ExperienceGroup.Slow:
                    {
                        maxExperience = (5 * (int)Mathf.Pow(level, 3)) / 4;
                        break;
                    }
                case ExperienceGroup.Fluctuating:
                    {
                        if (level <= 15)
                        {
                            maxExperience = (int)Mathf.Pow(level, 3) * (((int)Mathf.Floor((level + 1) / 3) + 24) / 50);
                        }
                        else if (level > 15 && level <= 36)
                        {
                            maxExperience = (int)Mathf.Pow(level, 3) * ((level + 14) / 50);
                        }
                        else if (level > 36 && level <= 100)
                        {
                            maxExperience = (int)Mathf.Pow(level, 3) * (((int)Mathf.Floor(level / 2) + 32) / 50);
                        }

                        break;
                    }
            }

            return maxExperience;
        }

        #endregion
    }

    [System.Serializable]
    public class PokemonNature
    {
        #region Fields

        [SerializeField] private Nature value;

        #endregion

        #region Properties

        public Nature Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public (Stat, Stat) ChangedStats
        {
            get
            {
                Stat increasedStat;
                Stat decreasedStat;

                if (value == Nature.Lonely || value == Nature.Brave || value == Nature.Adamant || value == Nature.Naughty)
                {
                    increasedStat = Stat.Attack;
                }
                else if (value == Nature.Bold || value == Nature.Relaxed || value == Nature.Impish || value == Nature.Lax)
                {
                    increasedStat = Stat.Defence;
                }
                else if (value == Nature.Timid || value == Nature.Hasty || value == Nature.Jolly || value == Nature.Naive)
                {
                    increasedStat = Stat.Speed;
                }
                else if (value == Nature.Modest || value == Nature.Mild || value == Nature.Quiet || value == Nature.Rash)
                {
                    increasedStat = Stat.SpAttack;
                }
                else if (value == Nature.Calm || value == Nature.Gentle || value == Nature.Sassy || value == Nature.Careful)
                {
                    increasedStat = Stat.SpDefence;
                }
                else
                {
                    increasedStat = Stat.None;
                }

                if (value == Nature.Lonely || value == Nature.Docile || value == Nature.Mild || value == Nature.Gentle)
                {
                    decreasedStat = Stat.Defence;
                }
                else if (value == Nature.Brave || value == Nature.Relaxed || value == Nature.Quiet || value == Nature.Sassy)
                {
                    decreasedStat = Stat.Speed;
                }
                else if (value == Nature.Adamant || value == Nature.Impish || value == Nature.Jolly || value == Nature.Careful)
                {
                    decreasedStat = Stat.SpAttack;
                }
                else if (value == Nature.Naughty || value == Nature.Lax || value == Nature.Naive || value == Nature.Rash)
                {
                    decreasedStat = Stat.SpDefence;
                }
                else if (value == Nature.Bold || value == Nature.Timid || value == Nature.Modest || value == Nature.Calm)
                {
                    decreasedStat = Stat.Attack;
                }
                else
                {
                    decreasedStat = Stat.None;
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
    public class PokemonAbility
    {
        
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
    }

    #endregion
}