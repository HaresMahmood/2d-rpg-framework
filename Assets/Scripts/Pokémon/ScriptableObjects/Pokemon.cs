using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokémon", menuName = "Characters/Pokémon")]
public class Pokemon : ScriptableObject
{
    #region Fields

    //[SerializeField] private string ability;

    #endregion

    #region Properties

    public string Name { get; set; }

    public int ID { get; set; }

    public PokemonProgression Progression { get; } = new PokemonProgression();

    public string Category { get; set; }

    public string DexEntry { get; set; }

    /*
    public int Ability
    {
        get { return level; }
        private set { level = value; }
    }
    */

    public PokemonSprites Sprites { get; private set; }

    public Typing PrimaryType { get; set; }

    public Typing SecondaryType { get; set; }

    public PokemonStats BaseStats { get; private set; }

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

    #endregion

    #region Nested Classes

    [System.Serializable]
    public class PokemonProgression
    {
        #region Properties

        public LevelingGroup Group { get; set; }

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

    [System.Serializable]
    public class PokemonAbility
    {
        
    }

    [System.Serializable]
    public class PokemonStats
    {
        #region Properties

        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int SpAttack { get; set; }
        public int SpDefence { get; set; }
        public int Speed { get; set; }

        #endregion
    }

    [System.Serializable]
    public class PokemonSprites
    {
        #region Properties

        public Sprite FrontSprite { get; set; }

        public Sprite BackSprite { get; set; }

        public Sprite MenuSprite { get; set; }

        #endregion
    }

    #endregion
}