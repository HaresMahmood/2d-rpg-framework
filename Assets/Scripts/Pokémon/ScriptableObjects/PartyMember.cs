using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Party Member", menuName = "Characters/Pokémon")]
public class PartyMember : ScriptableObject
{
    #region Fields

    [SerializeField] private static Pokemon pokemon;
    [SerializeField] private string nickname;
    [SerializeField] private Gender gender;
    [SerializeField] private MemberProgression progression = new MemberProgression(Pokemon);
    [SerializeField] private MemberMetAt metAt = new MemberMetAt();
    [SerializeField] private MemberNature nature = new MemberNature();
    [SerializeField] private List<Move> activeMoves = new List<Move>();
    [SerializeField] private List<Move> learnedMoves = new List<Move>();
    [SerializeField] private StatusAilment ailment = new StatusAilment();
    [SerializeField] private Item heldItem;
    [SerializeField] private MemberStats stats = new MemberStats();

    #endregion

    #region Properties

    public static Pokemon Pokemon
    {
        get { return pokemon; }
        set { pokemon = value; }
    }

    public string Nickname
    {
        get { return nickname; }
        set { nickname = value; }
    }

    public MemberProgression Progression
    {
        get { return progression; }
    }

    public MemberMetAt MetAt
    {
        get { return metAt; }
    }

    public MemberNature Nature
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

    public List<Move> ActiveMoves
    {
        get { return activeMoves; }
    }

    public List<Move> LearnedMoves
    {
        get { return learnedMoves; }
    }

    public StatusAilment Ailment
    {
        get { return ailment; }
    }

    public Item HeldItem
    {
        get { return heldItem; }
        set { heldItem = value; }
    }

    public MemberStats Stats
    {
        get { return stats; }
    }

    #endregion

    #region Enums

    public enum Gender
    {
        Male,
        Female
    } // TODO: Create nested class to calculate gender ratios.

    #endregion

    #region Nested Classes

    [Serializable]
    public class MemberProgression
    {
        #region Variables

        private Pokemon pokemon;

        #endregion

        #region Properties

        public int Value { get; set; }

        public int Level { get; set; }

        public int Total
        {
            get
            {
                return Mathf.Max(0, pokemon.Progression.GetTotalExperience(Level));
            }
        }

        public int Remaining
        {
            get
            {
                if (Level > 0 && Level < 100)
                {
                    return (pokemon.Progression.GetTotalExperience(Level + 1) - Total);
                }

                return 0;
            }
        }

        #endregion

        #region Constructor

        internal MemberProgression(Pokemon pokemon)
        {
            this.pokemon = pokemon;
        }

        #endregion
    }

    [Serializable]
    public class MemberMetAt
    {
        #region Variables

        [SerializeField] private int level;
        [SerializeField] private string location;

        #endregion

        #region Properties

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        #endregion
    }

    [Serializable]
    public class MemberNature
    {
        #region Properties

        public Nature Value { get; set; }

        /*
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
        */

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

    [Serializable]
    public class PokemonAbility
    {

    }

    [Serializable]
    public class StatusAilment
    {
        #region Fields

        private readonly Ailment value;

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

    [Serializable]
    public class MemberStats
    {
        #region Variables

        Pokemon.Stat[] values = Enum.GetValues(typeof(Pokemon.Stat)).Cast<Pokemon.Stat>().ToArray();

        #endregion

        #region Fields

        [SerializeField] private int hp;
        [SerializeField] private StatDictionary stats;
        [SerializeField] private StatDictionary evs;
        [SerializeField] private StatDictionary ivs;
        [SerializeField] private int happiness;

        #endregion

        #region Properties

        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        public Dictionary<Pokemon.Stat, int> Stats
        {
            get
            {
                if (stats.Count == 0)
                {
                    foreach (Pokemon.Stat stat in values)
                    {
                        stats.Add(stat, 0);
                    }
                }

                return stats;
            }
        }

        public Dictionary<Pokemon.Stat, int> EVs
        {
            get
            {
                if (evs.Count == 0)
                {
                    foreach (Pokemon.Stat stat in values)
                    {
                        stats.Add(stat, 0);
                    }
                }

                return evs;
            }
        }

        public Dictionary<Pokemon.Stat, int> IVs
        {
            get
            {
                if (stats.Count == 0)
                {
                    foreach (Pokemon.Stat stat in values)
                    {
                        ivs.Add(stat, UnityEngine.Random.Range(0, 32));
                    }
                }

                return ivs;
            }
        }

        public int Happiness
        {
            get { return happiness; }
            set { happiness = value; }
        }

        #endregion
    }

    [Serializable]
    public class StatDictionary : SerializableDictionary<Pokemon.Stat, int>
    { }

    #endregion
}