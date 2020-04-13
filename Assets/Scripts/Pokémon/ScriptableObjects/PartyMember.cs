using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Party Member", menuName = "Characters/Party Member")]
public class PartyMember : ScriptableObject
{
    #region Fields

    [SerializeField] private Pokemon species;
    [SerializeField] private string nickname;
    [SerializeField] private MemberGender gender = new MemberGender();
    [SerializeField] private MemberProgression progression;
    [SerializeField] private MemberMetAt metAt = new MemberMetAt();
    [SerializeField] private MemberNature nature = new MemberNature();
    [SerializeField] private List<Move> activeMoves = new List<Move>();
    [SerializeField] private List<Move> learnedMoves = new List<Move>();
    [SerializeField] private StatusAilment ailment = new StatusAilment();
    [SerializeField] private Item heldItem;
    [SerializeField] private MemberStats stats = new MemberStats();

    #endregion

    #region Properties

    public Pokemon Species
    {
        get { return species; }
        set { species = value; }
    }

    public string Nickname
    {
        get { return nickname; }
        set { nickname = value; }
    }

    public MemberGender Gender
    {
        get { return gender; }
    }

    public MemberProgression Progression
    {
        get 
        { 
            if (progression == null)
            {
                progression = new MemberProgression(Species);
            }

            return progression; 
        }
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

    #region Nested Classes

    [Serializable]
    public class MemberGender
    {
        #region Fields

        [SerializeField] private Gender value;

        #endregion

        #region Properties

        public Gender Value
        {
            get { return value; }
            set { this.value = value; }
        }

        #endregion

        #region Enums

        public enum Gender
        {
            Male,
            Female
        }

        #endregion

        #region Miscellaneous Methodss

        public Gender AssignGender(Pokemon species)
        {
            float[] probabilities = new float[2];
            probabilities[0] = species.GenderRatio;
            probabilities[1] = 100f - probabilities[0];
            float probability = UnityEngine.Random.Range(0f, 1f) * 100;
            float currentProbability = probabilities[0];
            Gender selectedGender = Gender.Male;

            for (int i = 0; i < 2; i++)
            {
                if (probability <= currentProbability)
                {
                    selectedGender = (Gender)i;
                    break;
                }

                currentProbability += probabilities[i + 1];
            }

            return selectedGender;
        }

        #endregion
    }

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

        public StatDictionary ModifiedStat
        {
            get
            {
                StatDictionary stats = new StatDictionary();
                Pokemon.Stat[] values = Enum.GetValues(typeof(Pokemon.Stat)).Cast<Pokemon.Stat>().ToArray();

                foreach (Pokemon.Stat stat in values)
                {
                    int value = 100;

                    if ((((Value == Nature.Lonely || Value == Nature.Brave || Value == Nature.Adamant || Value == Nature.Naughty)) && stat == Pokemon.Stat.Attack)
                    || (((Value == Nature.Bold || Value == Nature.Relaxed || Value == Nature.Impish || Value == Nature.Lax)) && stat == Pokemon.Stat.Defence)
                    || (((Value == Nature.Timid || Value == Nature.Hasty || Value == Nature.Jolly || Value == Nature.Naive)) && stat == Pokemon.Stat.Speed)
                    || (((Value == Nature.Modest || Value == Nature.Mild || Value == Nature.Quiet || Value == Nature.Rash)) && stat == Pokemon.Stat.SpAttack)
                    || (((Value == Nature.Calm || Value == Nature.Gentle || Value == Nature.Sassy || Value == Nature.Careful)) && stat == Pokemon.Stat.SpDefence))
                    {
                        value = 110;
                    }
                    else
                    if ((((Value == Nature.Lonely || Value == Nature.Docile || Value == Nature.Mild || Value == Nature.Gentle)) && stat == Pokemon.Stat.Defence)
                    || (((Value == Nature.Brave || Value == Nature.Relaxed || Value == Nature.Quiet || Value == Nature.Sassy)) && stat == Pokemon.Stat.Speed)
                    || (((Value == Nature.Adamant || Value == Nature.Impish || Value == Nature.Jolly || Value == Nature.Careful)) && stat == Pokemon.Stat.SpAttack)
                    || (((Value == Nature.Naughty || Value == Nature.Lax || Value == Nature.Naive || Value == Nature.Rash)) && stat == Pokemon.Stat.SpDefence)
                    || (((Value == Nature.Bold || Value == Nature.Timid || Value == Nature.Modest || Value == Nature.Calm)) && stat == Pokemon.Stat.Attack))
                    {
                        value = 90;
                    }

                    stats.Add(stat, value);
                }

                return stats;
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

        public StatDictionary Stats
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

        public StatDictionary EVs
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

        public StatDictionary IVs
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