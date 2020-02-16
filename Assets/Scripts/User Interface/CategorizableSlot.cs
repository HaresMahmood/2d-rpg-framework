using UnityEngine;

/// <summary>
///
/// </summary>
public abstract class CategorizableSlot : MonoBehaviour
{
    #region Variables

    protected Transform slot;

    #endregion

    #region Miscellaneous Methods

    public void AnimateSlot(float opacity, float duration = -1)
    {
        if (duration > -1)
        {
            StartCoroutine(gameObject.FadeOpacity(opacity, duration));
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = opacity;
        }

        if (opacity == 0f) gameObject.SetActive(false);
    }

    public void UpdateInformation(Categorizable categorizable, float duration = -1)
    {
        if (!slot.gameObject.activeSelf)
        {
            slot.gameObject.SetActive(true);
        }

        SetInformation(categorizable);

        AnimateSlot(1f, duration);
    }

    protected virtual void SetInformation(Categorizable categorizable)
    {   }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        slot.gameObject.SetActive(false);
    }

    #endregion
}
