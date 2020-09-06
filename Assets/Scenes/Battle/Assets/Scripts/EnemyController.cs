using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class EnemyController : MonoBehaviour
{
    #region Variables

    [SerializeField] private BattleUserInterface battleUserInterface;

    [Header("Values")]
    [SerializeField] private PartyMember enemy;

    #endregion

    #region Properties

    public PartyMember Enemy
    {
        set { enemy = value; }
    }

    #endregion
    
    #region Miscellaneous Methods

    public int Attack()
    {
        PartyMember.MemberMove move = enemy.ActiveMoves[UnityEngine.Random.Range(0, (enemy.ActiveMoves.Count))];

        Debug.Log(move.Value.Name);

        return move.Value.CalculateDamage(battleUserInterface.Partner, battleUserInterface.Enemy);
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

