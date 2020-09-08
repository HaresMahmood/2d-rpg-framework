using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public class BattleUserInterface : XUserInterface<Party>
{
    #region Properties

    public Party Information { set { information = value; } }

    #endregion

    #region Variables

    [Header("Settings")]
    [SerializeField] private float animationTime;

    private HealthSubComponent partnerHealth;
    private HealthSubComponent enemyHealth;

    #endregion

    #region Events

    #endregion

    #region Miscellaneous Methods

    private void SetPartner()
    {
        partnerHealth.SetInformation(BattleManager.Instance.Partner);
    }

    private void SetEnemy()
    {
        enemyHealth.SetInformation(BattleManager.Instance.Enemy);
    }

    #endregion

    #region Event Methods

    private void Battle_OnAttack(object sender, int damage)
    {
        HealthSubComponent healthComponent = BattleManager.Instance.Stage == BattleManager.BattleStage.Partner ? enemyHealth : partnerHealth;

        healthComponent.SetHealth();
    }

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        //damageText = transform.Find("Canvas (Damage)/Damage").GetComponent<DamageText>();

        partnerHealth = transform.Find("Canvas (UI)/Fighters/Partner").GetComponent<HealthSubComponent>();
        enemyHealth = transform.Find("Canvas (UI)/Fighters/Enemy").GetComponent<HealthSubComponent>();

        BattleManager.Instance.OnAttack += Battle_OnAttack;
    }

    protected override void Start()
    {
        base.Start();

        SetPartner();
        SetEnemy();
    }

    #endregion
}

