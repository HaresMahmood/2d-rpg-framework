using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region Variables

    [Header("Setup")]
    [SerializeField] private BattleUserInterface userInterface;

    [Header("Values")]
    [SerializeField] private BattleStage battleStage;

    #endregion

    #region Enums

    private enum BattleStage
    {
        //Start,
        Partner,
        Enemy,
        Won,
        Lost
    }

    #endregion

    #region Miscellaneous Methods



    #endregion

    #region Unity Methods

    private void Awake()
    {
        
    }

    private void Start()
    {
        //battleStage = BattleStage.Start;
    }

    private void Update()
    {
        
    }

    #endregion
}

