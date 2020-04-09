using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokémon", menuName = "Characters/Pokémon")]
public class Pokemon : ScriptableObject
{
    #region Fields

    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] internal int level;
    [SerializeField] private float experience;
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

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public float Experience
    {
        get { return experience; }
        set { experience = value; }
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
        #region Fields

        [SerializeField] private Category category;
        [SerializeField] private int maxExperience;
        private int level;

        #endregion

        #region Properties

        public Category Stats
        {
            get { return category; }
            private set { category = value; }
        }

        public int MaxExperience
        {
            get
            {
                int maxExperience = 0;

                switch (level)
                {
                    case (1):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (2):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (3):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (4):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (5):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (6):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (7):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (8):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (9):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (10):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (11):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (12):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (13):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (14):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (15):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (16):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (17):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (18):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (19):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (20):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (21):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (22):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (23):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (24):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (25):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (26):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (27):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                    case (28):
                        {
                            switch (category)
                            {
                                default: { break; }
                                case Category.Erratic:
                                    {

                                        break;
                                    }
                                case Category.Fast:
                                    {

                                        break;
                                    }
                                case Category.MediumFast:
                                    {

                                        break;
                                    }
                                case Category.MediumSlow:
                                    {

                                        break;
                                    }
                                case Category.Slow:
                                    {

                                        break;
                                    }
                                case Category.Fluctuating:
                                    {
                                        break;
                                    }
                            }

                            break;
                        }
                }

                return maxExperience;
            }
        }

        #endregion

        #region Enums

        public enum Category
        {
            Erratic,
            Fast,
            MediumFast,
            MediumSlow,
            Slow,
            Fluctuating
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