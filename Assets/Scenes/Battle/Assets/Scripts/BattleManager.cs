using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

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

    [Header("Values")]
    [SerializeField] private BattleStage stage;
    [Space(5)]
    [SerializeField] private PartyMember enemy;
    [SerializeField] private PartyMember partner;
    [Space(5)]
    [SerializeField] private PartyMember currentAttacker;

    #endregion

    #region Events

    public UnityEvent<BattleStage> OnBattleStageChange;

    public event EventHandler<int> OnAttack;
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
        OnAttack?.Invoke(this, damage);

        AttackComplete(damage);
    }

    public void AttackComplete(int damage)
    {
        currentAttacker = stage == BattleStage.Partner ? enemy : partner;
        currentAttacker.Stats.HP = Mathf.Clamp(currentAttacker.Stats.HP -= damage, 0, currentAttacker.Stats.HP);

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

    #region Event Methods



    #endregion

    #region Unity Methods

    private void Awake()
    {
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

