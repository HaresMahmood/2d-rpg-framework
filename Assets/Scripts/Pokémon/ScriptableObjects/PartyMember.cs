using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Party Member", menuName = "Characters/Pokémon")]
public class PartyMember : ScriptableObject
{

    #region Properties

    public static Pokemon Pokemon { get; set; }

    public string Nickname { get; set; }

    public MemberProgression Progression { get; } = new MemberProgression(Pokemon);

    public int MetAt { get; }

    public int HP { get; set; }

    public MemberNature Nature { get; } = new MemberNature();

    /*
    public int Ability
    {
        get { return level; }
        private set { level = value; }
    }
    */

    public List<Move> ActiveMoves { get; private set; } = new List<Move>();

    public List<Move> LearnedMoves { get; private set; } = new List<Move>();

    public StatusAilment Status { get; } = new StatusAilment();

    public Item HeldItem { get; set; }

    public MemberStats Stats { get; }

    #endregion

    #region Nested Classes

    [System.Serializable]
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

    [System.Serializable]
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

    [System.Serializable]
    public class PokemonAbility
    {

    }

    [System.Serializable]
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

    [System.Serializable]
    public class MemberStats
    {
        #region Variables

        Pokemon.Stat[] values = System.Enum.GetValues(typeof(Pokemon.Stat)).Cast<Pokemon.Stat>().ToArray();

        #endregion

        #region Fields

        //[SerializeField] private string ability;
        private Dictionary<Pokemon.Stat, int> stats = new Dictionary<Pokemon.Stat, int>();
        private Dictionary<Pokemon.Stat, int> ivs = new Dictionary<Pokemon.Stat, int>();
        private Dictionary<Pokemon.Stat, int> evs = new Dictionary<Pokemon.Stat, int>();

        #endregion

        #region Properties

        public Dictionary<Pokemon.Stat, int> Stats
        {
            get
            { 
                if (stats.Count != values.Length)
                {
                    foreach (Pokemon.Stat stat in values)
                    {
                        stats.Add(stat, 0);
                    }
                }

                return stats;
            }

            private set { stats = value; }
        }

        public Dictionary<Pokemon.Stat, int> IVs
        {
            get
            {
                if (ivs.Count != values.Length)
                {
                    foreach (Pokemon.Stat stat in values)
                    {
                        ivs.Add(stat, 0);
                    }
                }

                return stats;
            }

            private set { ivs = value; }
        }

        public Dictionary<Pokemon.Stat, int> Evs
        {
            get
            {
                if (evs.Count != values.Length)
                {
                    foreach (Pokemon.Stat stat in values)
                    {
                        evs.Add(stat, Random.Range(0, 32));
                    }
                }

                return stats;
            }

            private set { evs = value; }
        }

        #endregion
    }

    #endregion
}