using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
///
/// </summary>
public class BattleUserInterface : XUserInterface<Party>
{
    #region Variables

    [SerializeField] private EnemyController enemyAI;

    [Header("Values")]
    [SerializeField] private PartyMember enemy;
    [SerializeField] private PartyMember partner;
    [Space(5)]
    [SerializeField] private PartyMember currentAttacker;

    private DamageText damageText;

    private HealthSubComponent partnerHealth;
    private HealthSubComponent enemyHealth;

    private int currentPartner;

    #endregion

    #region Properties

    public PartyMember Enemy
    {
        get { return enemy; }
    }

    public PartyMember Partner
    {
        get { return partner; }
    }

    #endregion

    #region Miscellaneous Methods

    private void SetPartner()
    {
        partner = ((Party)Convert.ChangeType(information, typeof(Party))).playerParty[currentPartner];

        partnerHealth.SetInformation(partner);

        currentAttacker = partner;
    }

    private void SetEnemy()
    {
        enemyHealth.SetInformation(enemy);
    }

    private bool CheckBattleState()
    {
        return currentAttacker.Stats.HP > 0;
    }

    #endregion

    #region Event Methods

    private void Component_OnPartnerAttack(object sender, int damage)
    {
        if (CheckBattleState())
        {
            currentAttacker = partner;

            components.Find(c => c is MoveButtonComponent).GetComponent<MoveButtonComponent>().EnableButtons(false);

            damageText.AnimateText(damage);
            enemyHealth.SetHealth(damage);
        }
    }

    private void Component_OnEnemyAttack()
    {
        if (CheckBattleState())
        {
            currentAttacker = enemy;

            int damage = enemyAI.Attack();
            damageText.AnimateText(damage);
            partnerHealth.SetHealth(damage);
        }
    }

    private void DamageText_OnAnimationComplete(object sender, EventArgs e)
    {
        if (currentAttacker == enemy)
        {
            components.Find(c => c is MoveButtonComponent).GetComponent<MoveButtonComponent>().EnableButtons(true);
        }
        else
        {
            Component_OnEnemyAttack();
        }
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        damageText = transform.Find("Canvas (Damage)/Damage").GetComponent<DamageText>();

        partnerHealth = transform.Find("Canvas (UI)/Fighters/Partner").GetComponent<HealthSubComponent>();
        enemyHealth = transform.Find("Canvas (UI)/Fighters/Enemy").GetComponent<HealthSubComponent>();

        enemyAI.Enemy = enemy;
    }

    protected override void Start()
    {
        base.Start();

        SetPartner();
        SetEnemy();

        components.Find(c => c is MoveButtonComponent).GetComponent<MoveButtonComponent>().OnPartnerAttack += Component_OnPartnerAttack;
        damageText.OnAnimationComplete += DamageText_OnAnimationComplete;
    }

    #endregion
}

