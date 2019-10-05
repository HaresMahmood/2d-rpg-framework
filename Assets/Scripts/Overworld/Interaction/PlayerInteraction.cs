using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    #region Variables

    private static ContextController contextUI;
    public static GameObject contextBox;
    private static Animator animator;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        contextUI = GetComponentInChildren<ContextController>();
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
        contextBox.SetActive(false);
    }

    public static void UpdatePosition()
    {
        contextUI.UpdatePosition();
    }
}
