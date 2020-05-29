using System.Collections;
using UnityEngine;

public class PartyLearnedMovesController : PartyInformationController
{
    #region Variables

    private Animator animator;

    #endregion

    #region Miscellaneous Methods

    public void AnimatePanel(bool isActive)
    {
        float opacity = isActive ? 1f : 0f;

        animator.SetBool("isActive", isActive);
        StartCoroutine(gameObject.FadeOpacity(opacity, 0.15f)); // TODO: Make dynamic with animation time.
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
    }

    #endregion
}