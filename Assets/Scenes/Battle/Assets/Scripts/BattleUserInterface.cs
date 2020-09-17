using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public class BattleUserInterface : XUserInterface<Party>
{
    #region Variables

    [Header("Settings")]
    [SerializeField] private float animationTime;

    private PartySelectSubComponent partnerHealth;
    private PartySelectSubComponent enemyHealth;

    private AbilityComponent abilityComponent;

    #endregion

    #region Miscellaneous Methods

    private void SetPartner()
    {
        partnerHealth.SetInformation(((Party)Convert.ChangeType(information, typeof(Party))).playerParty[BattleManager.Instance.CurrentPartner]);
    }

    private void SetEnemy()
    {
        enemyHealth.SetInformation(BattleManager.Instance.Enemy);
    }

    #endregion

    #region Event Methods

    private void Battle_OnAttack(object sender, int damage)
    {
        PartySelectSubComponent healthComponent = BattleManager.Instance.Stage == BattleManager.BattleStage.Partner ? enemyHealth : partnerHealth;

        healthComponent.SetHealth();
    }

    private void Abilities_OnAbilityInvoke(object sender, PartyMember member)
    {
        abilityComponent.SetInformation(member.Ability, member == BattleManager.Instance.Partner);
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        //damageText = transform.Find("Canvas (Damage)/Damage").GetComponent<DamageText>();

        partnerHealth = transform.Find("Fighters/Partner").GetComponent<PartySelectSubComponent>();
        enemyHealth = transform.Find("Fighters/Enemy").GetComponent<PartySelectSubComponent>();
        abilityComponent = transform.Find("Fighters/Middle/Ability").GetComponent<AbilityComponent>();

        BattleManager.Instance.OnAttackComplete += Battle_OnAttack;
        AbilityController.OnAbilityInvoke += Abilities_OnAbilityInvoke;
    }

    protected override void Start()
    {
        base.Start();

        SetPartner();
        SetEnemy();

        UserInterfaceManager.Instance.PushUserInterface(gameObject);
    }

    #endregion
}

