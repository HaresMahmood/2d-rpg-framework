using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public class AbilityController : MonoBehaviour
{


    #region Events

    public static event EventHandler<PartyMember> OnAbilityInvoke;

    #endregion

    #region Miscellaneous Methods

    private void ChangeStat(PartyMember member, Pokemon.Stat stat, int amount)
    {
        member.Stats.StatChanges[stat] = Mathf.Clamp(member.Stats.StatChanges[stat] + amount, -6, 6);
    }

    #endregion


    #region Ability Behevior

    public void Intimidate(PartyMember currentMember)
    {
        PartyMember otherMember = currentMember == BattleManager.Instance.Partner ? BattleManager.Instance.Enemy : BattleManager.Instance.Partner;

        if (BattleManager.Instance.Stage == BattleManager.BattleStage.Start)
        {
            OnAbilityInvoke?.Invoke(this, currentMember);

            ChangeStat(otherMember, Pokemon.Stat.Attack, -1);
        }
    }

    public void SpeedBoost(PartyMember currentMember)
    {
        BattleManager.BattleStage memberStage = currentMember == BattleManager.Instance.Partner ? BattleManager.BattleStage.Partner : BattleManager.BattleStage.Enemy;

        if (BattleManager.Instance.Stage == BattleManager.BattleStage.Partner)
        {
            OnAbilityInvoke?.Invoke(this, currentMember);

            ChangeStat(currentMember, Pokemon.Stat.Speed, 1);
        }
    }

    #endregion

    #region Event Methods

    private void Battle_OnBattleStageChange(object sender, BattleManager.BattleStage e)
    {
        BattleManager.Instance.Partner.Ability?.Logic.Invoke((PartyMember)BattleManager.Instance.Partner);
        BattleManager.Instance.Enemy.Ability?.Logic.Invoke((PartyMember)BattleManager.Instance.Enemy);
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        BattleManager.Instance.OnBattleStageChange += Battle_OnBattleStageChange;
    }

    #endregion
}

