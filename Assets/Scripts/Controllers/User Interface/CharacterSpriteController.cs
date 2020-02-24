using UnityEngine;

/// <summary>
///
/// </summary>
public class CharacterSpriteController : MonoBehaviour
{
    #region Variables

    private Animator animator;

    #endregion

    #region Miscellaneous Methods

    public void SetAnimation(string activeMenu, string inactiveMenu)
    {
        animator.SetBool(activeMenu, true);
        animator.SetBool(inactiveMenu, false);
    }

    public void FadeSprite(float opacity, float animationDuration)
    {
        StartCoroutine(gameObject.FadeOpacity(opacity, animationDuration));
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #endregion
}
