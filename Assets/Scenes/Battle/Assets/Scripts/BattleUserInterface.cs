using System;

/// <summary>
///
/// </summary>
public class BattleUserInterface : XUserInterface<Party>
{
    #region Variables

    [UnityEngine.SerializeField] private PartyMember enemy;

    private HealthSubComponent partnerHealth;
    private HealthSubComponent opponentHealth;

    #endregion

    #region Miscellaneous Methods



    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        partnerHealth = transform.Find("Canvas/Fighters/Partner").GetComponent<HealthSubComponent>();
        opponentHealth = transform.Find("Canvas/Fighters/Enemy").GetComponent<HealthSubComponent>();
    }

    protected override void Start()
    {
        base.Start();

        partnerHealth.SetInformation(((Party)Convert.ChangeType(information, typeof(Party))).playerParty[0]);
        opponentHealth.SetInformation(enemy);
    }

    #endregion
}

