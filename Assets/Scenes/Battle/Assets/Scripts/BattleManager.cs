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
    [SerializeField] private PartyMember partner;
    [Space(5)]
    [SerializeField] private PartyMember currentAttacker;

    #endregion

    #region Events
    
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
        Won,
        Lost
    }

    #endregion

    #region Miscellaneous Methods

    public void Attack(int damage)
    {
        Debug.Log(animationControllers[(int)stage]);

        StartCoroutine(animationControllers[(int)stage].Attack(damage));
    }

    public void AttackComplete(int damage)
    {
        currentAttacker = Stage == BattleStage.Partner ? enemy : partner;
        currentAttacker.Stats.HP = Mathf.Clamp(currentAttacker.Stats.HP -= damage, 0, currentAttacker.Stats.HP);

        OnAttackComplete?.Invoke(this, damage);

        ChangeBattleStage();
    }

    private void ChangeBattleStage()
    {
        if (currentAttacker.Stats.HP <= 0)
        {
            if (currentAttacker == partner)
            {
                stage = BattleStage.Lost;
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
                stage = BattleStage.Won;
                OnEnemyFaint?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            stage = (BattleStage)ExtensionMethods.IncrementInt((int)stage, 0, 2, 1);
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        //animationControllers = FindObjectsOfType<BattleAnimationController>().ToList();

        userInterface.Information = party;
    }

    private void Start()
    {
        stage = BattleStage.Partner;

        partner = party.playerParty[CurrentPartner];
        currentAttacker = partner;
    }

    #endregion
}

