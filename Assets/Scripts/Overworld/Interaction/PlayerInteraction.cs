using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    #region Variables

    private ClampUI contextUI;
    public static GameObject contextBox;
    private static Animator animator;

    #endregion
    
    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        contextUI = GetComponentInChildren<ClampUI>();
        contextBox = contextUI.contextBox;
        animator = contextBox.GetComponent<Animator>();
    }

    #endregion

    public static void SetVisible()
    {
        contextBox.SetActive(true);
    }

    public static IEnumerator SetInvisible()
    {
        animator.SetTrigger("isInactive");
        yield return new WaitForSeconds(animator.GetAnimationTime());
        //yield return null;
        contextBox.SetActive(false);
    }
}
