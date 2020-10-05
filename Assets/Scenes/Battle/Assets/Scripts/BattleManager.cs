using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region Fields

    private static BattleManager instance;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BattleManager>();
            }

            return instance;
        }
    }

    public BattleStage Stage
    {
        get { return stage; }
        set { stage = value; OnBattleStageChange?.Invoke(this, stage); }
    }

    public PartyMember Enemy { get { return enemy; } }

    public PartyMember Partner { get { return partner; } }

    public int CurrentPartner { get; }

    #endregion

    #region Variables

    [Header("Setup")]
    [SerializeField] private Party party;
    [SerializeField] private BattleUserInterface userInterface;
    [SerializeField] private List<BattleAnimationController> animationControllers;

    [Header("Values")]
    [SerializeField] private BattleStage stage;
    [Space(5)]
    [SerializeField] private PartyMember enemy;
    [SerializeField, ReadOnly] private PartyMember partner;
    [Space(5)]
    [SerializeField] private PartyMember currentAttacker;

    #endregion

    #region Events

    //[Header("Events")] [Space(5)] [SerializeField]
    public event EventHandler<BattleStage> OnBattleStageChange;
    
    public event EventHandler<int> OnAttack;
    public event EventHandler<int> OnAttackComplete;
    public event EventHandler OnPartnerFaint;
    public event EventHandler OnEnemyFaint;

    #endregion

    #region Enums

    public enum BattleStage
    {
        Partner,
        Enemy,
        Start,
        Won,
        Lost
    }

    #endregion

    #region Miscellaneous Methods

    public void Attack(PartyMember.MemberMove move)
    {
        StartCoroutine(animationControllers[(int)Stage].Attack(move));
    }

    public void AttackComplete(PartyMember.MemberMove move)
    {
        currentAttacker = Stage == BattleStage.Partner ? enemy : partner;

        if (move.Value.Type == Move.MoveType.Regular)
        {
            currentAttacker.Stats.HP = Mathf.Clamp(currentAttacker.Stats.HP -= move.Value.CalculateDamage(partner, enemy), 0, currentAttacker.Stats.HP);
        }
        else
        {
            ChangeStat(currentAttacker == partner ? enemy : partner, move.Value.ChangedStat, move.Value.ChangedStatAmount);
        }

        OnAttackComplete?.Invoke(this, move.Value.CalculateDamage(partner, enemy)); // TODO: Don't pass in damage

        ChangeBattleStage();
    }

    private void ChangeBattleStage()
    {
        if (currentAttacker.Stats.HP <= 0)
        {
            if (currentAttacker == partner)
            {
                Stage = BattleStage.Lost;
                OnPartnerFaint?.Invoke(this, EventArgs.Empty);

                /*
                if (CurrentPartner < party.playerParty.Count)
                {
                    // TODO: Bring up party select screen.
                }
                else
                {
                    battleStage = BattleStage.Lost;
                    OnPartnerFaint?.Invoke(this, EventArgs.Empty);
                }
                */
            }
            else
            {
                Stage = BattleStage.Won;
                OnEnemyFaint?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            Stage = (BattleStage)ExtensionMethods.IncrementInt((int)Stage, 0, 2, 1);
        }
    }

    private void ChangeStat(PartyMember member, Pokemon.Stat stat, int amount)
    {
        member.Stats.StatChanges[stat] = Mathf.Clamp(member.Stats.StatChanges[stat] + amount, -6, 6);
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        partner = party.playerParty[CurrentPartner];
        currentAttacker = partner;

        Stage = BattleStage.Start;
        Stage = BattleStage.Partner;
    }

    private void OnDisable()
    {
        // TODO: Should actually happen at end of battle
        partner.Stats.ResetStatChanges();
        enemy.Stats.ResetStatChanges();
    }

    private void OnApplicationQuit()
    {
        // TODO: Should actually happen at end of battle
        partner.Stats.ResetStatChanges();
        enemy.Stats.ResetStatChanges();
    }

    #endregion
}

