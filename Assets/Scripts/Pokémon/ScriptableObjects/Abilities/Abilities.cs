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

    public void Intimidate()
    {
        if (BattleManager.Instance.Stage == BattleManager.BattleStage.Start)
        {
            OnAbilityInvoke?.Invoke(this, BattleManager.Instance.Partner);

            Mathf.Clamp(BattleManager.Instance.Enemy.Stats.StatChanges[Pokemon.Stat.Attack] - 1, -6, 6);
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

