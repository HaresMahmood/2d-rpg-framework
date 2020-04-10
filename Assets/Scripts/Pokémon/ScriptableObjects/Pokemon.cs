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
    [SerializeField] private PokemonProgression progression = new PokemonProgression();
    [SerializeField] private string category;
    [SerializeField] private string dexEntry;
    [SerializeField] private PokemonSprites sprites = new PokemonSprites();
    [SerializeField] private Typing primaryType;
    [SerializeField] private Typing secondaryType;
    [SerializeField] private StatDictionary baseStats;
    

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
    }

    public Dictionary<Stat, int> BaseStats
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
    public class PokemonProgression
    {
        #region Fields

        private LevelingGroup group;

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
            int maxExperience = 0;

            switch (Group)
            {
                default: { break; }
                case LevelingGroup.Erratic:
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
                case LevelingGroup.Fast:
                    {
                        maxExperience = 4 * (int)Mathf.Pow(level, 3) / 5;
                        break;
                    }
                case LevelingGroup.MediumFast:
                    {
                        maxExperience = level ^ 3;
                        break;
                    }
                case LevelingGroup.MediumSlow:
                    {
                        maxExperience = ((int)(1.2f * (int)Mathf.Pow(level, 3))) - (15 * (int)Mathf.Pow(level, 2)) + (100 * level) - 140;
                        break;
                    }
                case LevelingGroup.Slow:
                    {
                        maxExperience = (5 * (int)Mathf.Pow(level, 3)) / 4;
                        break;
                    }
                case LevelingGroup.Fluctuating:
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

    [Serializable]
    public class PokemonAbility
    {
        
    }

    [Serializable]
    public class PokemonSprites
    {
        #region Fields

        private Sprite frontSprite;
        private Sprite backSprite;
        private Sprite menuSprite;

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

    [Serializable] 
    public class StatDictionary : SerializableDictionary<Stat, int> 
    { }

    #endregion
}