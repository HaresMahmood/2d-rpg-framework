﻿using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Move", menuName = "Moves/Move")]
public class Move : ScriptableObject
{
    #region Fields

    [SerializeField] private new string name;
    [SerializeField] private int id;
    [SerializeField] private string description;
    [SerializeField] private int pp;
    [SerializeField] private int accuracy;
    [SerializeField] private int power;
    [SerializeField] private MoveType type;
    [SerializeField] private MoveTarget target;
    [SerializeField] private MoveCategory category;
    [SerializeField] private Typing typing;
    [SerializeField] private UnityEvent moveEvent;

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

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public int PP
    {
        get { return pp; }
        set { pp = value; }
    }

    public int Accuracy
    {
        get { return accuracy; }
        set { accuracy = value; }
    }

    public int Power
    {
        get { return power; }
        set { power = value; }
    }

    public MoveType Type
    {
        get { return type; }
        set { type = value; }
    }

    public MoveTarget Target
    {
        get { return target; }
        set { target = value; }
    }

    public MoveCategory Category
    {
        get { return category; }
        set { category = value; }
    }

    public Typing Typing
    {
        get { return typing; }
    }

    public UnityEvent MoveEvent
    {
        get { return moveEvent; }
        set { moveEvent = value; }
    }

    #endregion

    #region Enums

    public enum MoveTarget
    {
        User,
        Enemy,
        Everyone
    }

    public enum MoveType
    { 
        Regular,
        Stat
    }

    public enum MoveCategory
    {
        Special,
        Physical
    }

    #endregion

    #region Miscellaneous Methods

    public int CalculateDamage(PartyMember partner, PartyMember enemy)
    {
        float attackStat = category == Move.MoveCategory.Physical 
            ? (partner.Stats.Stats[Pokemon.Stat.Attack] * GetStatChangeModifier(partner.Stats.StatChanges[Pokemon.Stat.Attack]))
            : (partner.Stats.Stats[Pokemon.Stat.SpAttack] * GetStatChangeModifier(partner.Stats.StatChanges[Pokemon.Stat.SpAttack]));

        float defenceStat = category == Move.MoveCategory.Physical 
            ? (enemy.Stats.Stats[Pokemon.Stat.Defence] * GetStatChangeModifier(enemy.Stats.StatChanges[Pokemon.Stat.Defence])) 
            : (enemy.Stats.Stats[Pokemon.Stat.SpDefence] * GetStatChangeModifier(enemy.Stats.StatChanges[Pokemon.Stat.SpDefence]));

        float modifier = 0.75f // Target (default: one target)
                       * 1f // Weather (default: neutral weather)
                       * 1f // Critical (default: non-critical) https://bulbapedia.bulbagarden.net/wiki/Critical_hit
                       * UnityEngine.Random.Range(0.85f, 1f) // Random
                       * (partner.Species.PrimaryType == typing || partner.Species.SecondaryType == typing ? 1.5f : 1f) // STAB
                       * 1f // Type (default: normally effective type) https://youtu.be/PvtHnmSy9qc?t=371
                       * 1f // Burn (default: target not burned)
                       * 1f; // Other (default: "1 in most cases")
                             // https://bulbapedia.bulbagarden.net/wiki/Damage

        float damage = (((float)(2 * partner.Progression.Level) / 5) + 2) * power * (float)attackStat / (float)defenceStat / 50 * modifier;

        return Mathf.FloorToInt(damage);
    }

    private int GetStatChangeModifier(int statChange)
    {
        Debug.Log(Mathf.Max(2, 2 + statChange) + " / " +  Mathf.Max(2, 2 + statChange));

        return Mathf.Max(2, 2 + statChange) / Mathf.Max(2, 2 - statChange);
    }

    #endregion
}