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

    private bool CheckBattleState()
    {
        return currentAttacker.Stats.HP > 0;
    }

    #endregion

    #region Event Methods

    private void Component_OnPartnerAttack(object sender, List<int> list)
    {
        currentAttacker = partner;

        damageText.AnimateText(list[0].ToString(), list[1]);
        enemyHealth.SetHealth(list[0]);

        if (CheckBattleState())
        {
            Component_OnEnemyAttack();
        }
    }

    private void Component_OnEnemyAttack()
    {
        currentAttacker = enemy;

        //damageText.SetText(damage.ToString());

        (int damage, int power) = enemyAI.Attack();

        partnerHealth.SetHealth(damage);
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

        partner = (((Party)Convert.ChangeType(information, typeof(Party))).playerParty[0]);

        partnerHealth.SetInformation(partner);
        enemyHealth.SetInformation(enemy);

        components.Find(c => c is MoveButtonUserInterface).GetComponent<MoveButtonUserInterface>().OnPartnerAttack += Component_OnPartnerAttack;
    }

    #endregion
}

