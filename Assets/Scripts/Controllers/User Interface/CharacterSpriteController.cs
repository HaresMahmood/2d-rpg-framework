using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class CharacterSpriteController : MonoBehaviour
{
    #region Fields

    private static CharacterSpriteController instance;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static CharacterSpriteController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterSpriteController>(); // TODO: "FindObjectOfType" = bad performance. Think of better.
            }

            return instance;
        }
    }

    #endregion

    #region Variables

    private Animator animator;

    #endregion

    #region Miscellaneous Methods

    public void SetAnimation(string activeMenu, string inactiveMenu)
    {
        animator.SetBool(activeMenu, true);
        animator.SetBool(inactiveMenu, false);
    }

    public void SetSprite(Sprite sprite)
    {
        transform.Find("Pokémon/Sprite").GetComponent<Image>().sprite = sprite; // TODO: Debug
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
