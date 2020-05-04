using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokémon", menuName = "Characters/Pokémon")]
public class Pokemon : ScriptableObject
{
    #region Fields

    //[SerializeField] private string ability;
    [SerializeField] private new string name;
    [SerializeField] private int id;
    [SerializeField] private string category;
    [SerializeField] private string dexEntry;
    [SerializeField] private float genderRatio;
    [SerializeField] private int catchRate;
    [SerializeField] private Typing primaryType;
    [SerializeField] private Typing secondaryType;
    [SerializeField] private PokemonMeasurements measurements = new PokemonMeasurements();
    [SerializeField] private PokemonProgression progression = new PokemonProgression();
    [SerializeField] private PokemonStats stats = new PokemonStats();
    [SerializeField] private PokemonYield yield = new PokemonYield();
    [SerializeField] private PokemonSprites sprites = new PokemonSprites();

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

    public PokemonProgression Progression
    {
        get { return progression; }
    }

    public PokemonMeasurements Measurements
    {
        get { return measurements; }
    }

    public string Category
    {
        get { return category; }
        set { category = value; }
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

    public string DexEntry
    {
        get { return dexEntry; }
        set { dexEntry = value; }
    }

    public float GenderRatio
    {
        get { return genderRatio; }
        set { genderRatio = value; }
    }

    public int CatchRate
    {
        get { return catchRate; }
        set { catchRate = value; }
    }

    /*
    public int Ability
    {
        get { return level; }
        private set { level = value; }
    }
    */

    public PokemonStats Stats
    {
        get { return stats; }
    }

    public PokemonYield Yield
    {
        get { return yield; }
    }

    public PokemonSprites Sprites
    {
        get { return sprites; }
    }
    #endregion

    #region Enums

    public enum Stat
    {
        HP,
        Attack,
        Defence,
        SpAttack,
        SpDefence,
        Speed
    }

    #endregion

    #region Nested Classes

    [Serializable]
    public class PokemonMeasurements
    {
        #region Fields

        [SerializeField] private double height;
        [SerializeField] private double weight;

        #endregion

        #region Properties

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        #endregion
    }

    [Serializable]
    public class PokemonProgression
    {
        #region Fields

        [SerializeField]  private LevelingGroup group;

        #endregion

        #region Properties

        public LevelingGroup Group
        {
            get { return group; }
            set { group = value; }
        }

        #endregion

        #region Enums

        public enum LevelingGroup
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

        public int GetTotalExperience(int level)
        {
            int totalExperience = 0;

            if (level > 1)
            {
                switch (Group)
                {
                    default: { break; }
                    case LevelingGroup.Erratic:
                        {
                            if (level <= 50)
                            {
                                totalExperience = (int)Mathf.Pow(level, 3) * (100 - level) / 50;
                            }
                            else if (level > 50 && level <= 68)
                            {
                                totalExperience = (int)Mathf.Pow(level, 3) * (150 - level) / 100;
                            }
                            else if (level > 68 && level <= 98)
                            {
                                totalExperience = (int)Mathf.Pow(level, 3) * ((1911 - 10 * level) / 3) / 500;
                            }
                            else if (level > 98 && level <= 100)
                            {
                                totalExperience = (int)Mathf.Pow(level, 3) * (160 - level) / 100;
                            }

                            break;
                        }
                    case LevelingGroup.Fast:
                        {
                            totalExperience = 4 * (int)Mathf.Pow(level, 3) / 5;
                            break;
                        }
                    case LevelingGroup.MediumFast:
                        {
                            totalExperience = (int)Mathf.Pow(level, 3);
                            break;
                        }
                    case LevelingGroup.MediumSlow:
                        {
                            totalExperience = ((int)(1.2f * (int)Mathf.Pow(level, 3))) - (15 * (int)Mathf.Pow(level, 2)) + (100 * level) - 140;
                            break;
                        }
                    case LevelingGroup.Slow:
                        {
                            totalExperience = (5 * (int)Mathf.Pow(level, 3)) / 4;
                            break;
                        }
                    case LevelingGroup.Fluctuating:
                        {
                            if (level <= 15)
                            {
                                totalExperience = (int)((int)Mathf.Pow(level, 3) * ((Mathf.FloorToInt(((level + 1) / 3)) + 24) / 50.0));
                            }
                            else if (level > 15 && level <= 36)
                            {
                                totalExperience = (int)((int)Mathf.Pow(level, 3) * ((level + 14) / 50.0));
                            }
                            else if (level > 36 && level <= 100)
                            {
                                totalExperience = (int)((int)Mathf.Pow(level, 3) * (((Mathf.FloorToInt(level / 2) + 32) / 50.0)));
                            }

                            break;
                        }
                }
            }

            return totalExperience;
        }

        #endregion
    }

    [Serializable]
    public class PokemonAbility
    {
        
    }

    [Serializable]
    public class PokemonStats
    {
        #region Fields

        [SerializeField] private StatDictionary baseStats;
        [SerializeField] private int baseHappiness;

        #endregion

        #region Property

        public StatDictionary BaseStats
        {
            get
            {
                if (baseStats.Count == 0)
                {
                    IEnumerable values = Enum.GetValues(typeof(Stat)).Cast<Stat>();

                    foreach (Stat stat in values)
                    {
                        baseStats.Add(stat, 0);
                    }
                }

                return baseStats;
            }
        }

        public int BaseHappiness
        {
            get { return baseHappiness; }
            set { baseHappiness = value; }
        }

        #endregion
    }

    [Serializable]
    public class PokemonYield
    {
        #region Fields

        [SerializeField] private int experience;
        [SerializeField] private StatDictionary ev;

        #endregion

        #region Properties

        public int Experience
        {
            get { return experience; }
            set { experience = value; }
        }

        public StatDictionary EV
        {
            get
            {
                if (ev.Count == 0)
                {
                    IEnumerable values = Enum.GetValues(typeof(Stat)).Cast<Stat>();

                    foreach (Stat stat in values)
                    {
                        ev.Add(stat, 0);
                    }
                }

                return ev;
            }
        }

        #endregion
    }

    [Serializable]
    public class PokemonSprites
    {
        #region Fields

        [SerializeField] private Sprite menuSprite;
        [SerializeField] private Sprite frontSprite;
        [SerializeField] private Sprite backSprite;

        #endregion

        #region Properties

        public Sprite MenuSprite
        {
            get { return menuSprite; }
            set { menuSprite = value; }
        }

        public Sprite FrontSprite
        {
            get { return frontSprite; }
            set { frontSprite = value; }
        }

        public Sprite BackSprite
        {
            get { return backSprite; }
            set { backSprite = value; }
        }

        #endregion
    }

    [Serializable] 
    public class StatDictionary : SerializableDictionary<Stat, int> 
    { }

    #endregion
}