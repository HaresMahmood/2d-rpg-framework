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
    [SerializeField] private PokeBall pokeBall;
    [SerializeField] private MemberProgression progression = new MemberProgression();
    [SerializeField] private MemberMetAt metAt = new MemberMetAt();
    [SerializeField] private MemberNature nature = new MemberNature();
    [SerializeField] private List<MemberMove> activeMoves = new List<MemberMove>();
    [SerializeField] private List<MemberMove> learnedMoves = new List<MemberMove>();
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

    public PokeBall PokeBall
    {
        get { return pokeBall; }
        set { pokeBall = value; }
    }

    public MemberProgression Progression
    {
        get 
        {
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

    public List<MemberMove> ActiveMoves
    {
        get { return activeMoves; }
    }

    public List<MemberMove> LearnedMoves
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

        public Gender AssignRandom(Pokemon species)
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
        #region Fields

        [SerializeField] private int value;
        [SerializeField] private int level;
         
        #endregion

        #region Properties

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

        #endregion

        #region Miscellaneous Methods

        public int GetTotal(Pokemon species)
        {
            return Mathf.Max(0, species.Progression.GetTotalExperience(Level));

        }

        public int GetRemaining(Pokemon species)
        {
            if (Level > 0 && Level < 100)
            {
                return (species.Progression.GetTotalExperience(Level + 1) - GetTotal(species));
            }

            return 0;
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
        #region Fields

        [SerializeField] private Nature value;

        #endregion

        #region Properties

        public Nature Value 
        {
            get { return value; }
            set { this.value = value; }
        }

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

        #region Miscellaneous Methods

        public Nature AssignRandom()
        {
            Nature nature = (Nature)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(Nature)).Length));

            return nature;
        }

        #endregion
    }

    [Serializable]
    public class PokemonAbility
    {

    }

    [Serializable]
    public class MemberMove
    {
        #region Fields

        [SerializeField] private Move value;
        [SerializeField] private int pp;

        #endregion

        #region Properties

        public Move Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public int PP
        {
            get { return pp; }
            set { pp = value; }
        }

        #endregion
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
                        Debug.Log(stat);
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
                        evs.Add(stat, 0);
                    }
                }

                return evs;
            }
        }

        public StatDictionary IVs
        {
            get
            {
                if (ivs.Count == 0)
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

        #region Miscellaneous Methods

        public int CalculateStat(int baseStat, int iv, int ev, int level, int nature, bool calculateHP)
        {
            int stat = 0;

            if (calculateHP)
            {
                stat = Mathf.FloorToInt((2 * baseStat + iv + Mathf.Floor(ev / 4.0f)) * level / 100) + level + 10;
            }
            else
            {
               stat = Mathf.FloorToInt((Mathf.FloorToInt((2 * baseStat + iv + Mathf.Floor(ev / 4.0f)) * level / 100) + 5) * (nature / 100.0f));
            }

            return stat;
        }

        public void ResetEVs()
        {
            foreach (Pokemon.Stat stat in values)
            {
                evs[stat] = 0;
            }
        }

        public void AssignRandomIVs()
        {
            foreach (Pokemon.Stat stat in values)
            {
                ivs[stat] = UnityEngine.Random.Range(0, 32);
            }
        }

        #endregion
    }

    [Serializable]
    public class StatDictionary : SerializableDictionary<Pokemon.Stat, int>
    { }

    #endregion
}