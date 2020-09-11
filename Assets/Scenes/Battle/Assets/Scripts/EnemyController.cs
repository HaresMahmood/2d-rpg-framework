using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class EnemyController : MonoBehaviour
{   
    #region Miscellaneous Methods

    public void Attack()
    {
        PartyMember.MemberMove move = BattleManager.Instance.Enemy.ActiveMoves[UnityEngine.Random.Range(0, (BattleManager.Instance.Enemy.ActiveMoves.Count))];

        Debug.Log(move.Value.Name);

        BattleManager.Instance.Attack(move.Value.CalculateDamage(BattleManager.Instance.Partner, BattleManager.Instance.Enemy));
    }

    #endregion
}

