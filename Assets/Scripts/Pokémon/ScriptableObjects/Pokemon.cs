using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokémon", menuName = "Characters/Pokémon")]
public class Pokemon : ScriptableObject
{
    public new string name;
    public int id;
    public int level;
    [Range(0f, 200f)] public float exp;
    public float totalExp = 200;
    public string category;
    public string dexEntry;
    public string ability;
    public string nature; // TODO: Should be enum.
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite menuSprite;
    public Type primaryType;
    public Type secondaryType;
    public List<LearnedMove> learnedMoves = new List<LearnedMove>();
    public Status status;
    public Item heldItem;
    public Stats stats;

    [System.Serializable]
    public class Stats
    {
        public int hp;
        public int attack;
        public int defence;
        public int spAttack;
        public int spDefence;
        public int speed;

        private enum StatType
        {
            HP,
            Attack,
            Defence,
            SpAttack,
            SpDefence,
            Speed
        }

        /*
        public Stats(int hp, int attack, int defence, int spAttack, int spDefence, int speed)
        {
            this.hp = new Stat(hp);
            this.attack = new Stat(attack);
            this.defence = new Stat(defence);
            this.spAttack = new Stat(spAttack);
            this.spDefence = new Stat(spDefence);
            this.speed = new Stat(speed);
        }
        

        private Stat GetStat(StatType statType)
        {
            switch (statType)
            {
                default:
                case StatType.HP:           return hp;
                case StatType.Attack:       return attack;
                case StatType.Defence:      return defence;
                case StatType.SpAttack:     return spAttack;
                case StatType.SpDefence:    return spDefence;
                case StatType.Speed:        return speed;

            }
        }
        */

        public class Stat
        {
            public int stat;

            public Stat(int amount)
            {
                stat = amount;
            }
        }
    }

    [System.Serializable]
    public class LearnedMove
    {
        public Move move;
        public int remainingPp;
    }

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

    public enum Status
    {
        None,
        Paralyzed,
        Burned,
        Frozen,
        Poisoned,
        Asleep
    }
}