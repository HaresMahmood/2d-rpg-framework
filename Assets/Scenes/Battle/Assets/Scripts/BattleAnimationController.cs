using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class BattleAnimationController : MonoBehaviour
{
    #region Variables

    [SerializeField] private BattleManager.BattleStage stage;

    private DamageText damage;

    #endregion

    #region Miscellaneous Methods



    #endregion

    #region Event Methods

    private void Battle_OnAttack(object sender, int damage)
    {
        if (BattleManager.Instance.Stage == stage)
        {
            this.damage.Damage = damage;
            GetComponent<Animator>().SetTrigger("Attack");
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        BattleManager.Instance.OnAttack += Battle_OnAttack;

        damage = GetComponentInChildren<DamageText>();
    }

    #endregion
}

