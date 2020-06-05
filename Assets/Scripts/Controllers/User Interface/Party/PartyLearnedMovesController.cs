using System.Collections;
using UnityEngine;

public class PartyLearnedMovesController : PartyMovesPanelController
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

        UserInterface.UpdateSelectedObject(selectedValue, 0);
        UserInterface.UpdateSelectedObject(0);
        userInterface.ActivateSlot(0, false);

        selectedValue = 0;
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