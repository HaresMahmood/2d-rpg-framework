using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class EnemyController : MonoBehaviour
{   
    #region Miscellaneous Methods

    public int Attack()
    {
        PartyMember.MemberMove move = BattleManager.Instance.Enemy.ActiveMoves[UnityEngine.Random.Range(0, (BattleManager.Instance.Enemy.ActiveMoves.Count))];

        Debug.Log(move.Value.Name);

        return move.Value.CalculateDamage(BattleManager.Instance.Partner, BattleManager.Instance.Enemy);
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    #endregion
}

