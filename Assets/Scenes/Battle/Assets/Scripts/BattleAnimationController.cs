using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///
/// </summary>
public class BattleAnimationController : MonoBehaviour
{
    #region Variables

    [SerializeField] private BattleManager.BattleStage stage;

    private DamageText damageText;

    #endregion

    #region Events

    [Header("Events")] [Space(5)]
    [SerializeField] private UnityEvent OnAttackComplete;

    #endregion

    #region Miscellaneous Methods

    public IEnumerator Attack(int damage)
    {
        yield return new WaitForSeconds(0.5f);

        BattleManager.Instance.AttackComplete(damage);
        OnAttackComplete.Invoke();
    }

    #endregion
}

