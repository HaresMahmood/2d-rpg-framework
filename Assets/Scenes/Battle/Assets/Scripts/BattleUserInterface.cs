using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public class BattleUserInterface : XUserInterface<Party>
{
    #region Variables

    [SerializeField] public GameObject damageText;

    [Header("Values")]
    [SerializeField] private PartyMember enemy;
    [SerializeField] private PartyMember partner;

    private HealthSubComponent partnerHealth;
    private HealthSubComponent opponentHealth;

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



    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        damageText = transform.Find("Canvas (Damage)/Damage").gameObject;
        //damageText.SetActive(false);

        partnerHealth = transform.Find("Canvas (UI)/Fighters/Partner").GetComponent<HealthSubComponent>();
        opponentHealth = transform.Find("Canvas (UI)/Fighters/Enemy").GetComponent<HealthSubComponent>();
    }

    protected override void Start()
    {
        base.Start();

        partner = (((Party)Convert.ChangeType(information, typeof(Party))).playerParty[0]);

        partnerHealth.SetInformation(partner);
        opponentHealth.SetInformation(enemy);
    }

    #endregion
}

