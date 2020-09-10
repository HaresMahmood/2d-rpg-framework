using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public class Abilities : MonoBehaviour
{


    #region Events

    public static event EventHandler<PartyMember> OnAbilityInvoke;

    #endregion

    #region Miscellaneous Methods

    private void ChangeStat(PartyMember member, Pokemon.Stat stat, int amount)
    {
        Mathf.Clamp(BattleManager.Instance.Enemy.Stats.StatChanges[stat] + amount, -6, 6);
    }

    #endregion


    #region Ability Behevior

    public void Intimidate()
    {
        if (BattleManager.Instance.Stage == BattleManager.BattleStage.Start)
        {
            OnAbilityInvoke?.Invoke(this, BattleManager.Instance.Partner);

            ChangeStat(BattleManager.Instance.Enemy, Pokemon.Stat.Attack, -1);
        }
    }


    #endregion

    #region Event Methods

    private void Battle_OnBattleStageChange(object sender, BattleManager.BattleStage e)
    {
        BattleManager.Instance.Partner.Ability?.Logic.Invoke();
        //BattleManager.Instance.Enemy.Ability?.Logic.Invoke();
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        BattleManager.Instance.OnBattleStageChange += Battle_OnBattleStageChange;
    }

    #endregion
}

