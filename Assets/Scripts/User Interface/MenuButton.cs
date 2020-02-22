using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class MenuButton : MonoBehaviour
{
    #region Variables

    private Animator animator;

    private TextMeshProUGUI valueText;

    private Image smallIcon;
    private Image bigIcon;

    #endregion

    #region Miscellaneous Methods

    public void SetButtonInformation(string value, Sprite icon)
    {
        valueText.SetText(value);

        smallIcon.sprite = icon;
        bigIcon.sprite = icon;
    }

    public void FadeButton(float opacity, float animationDuration = 0.1f)
    {
        StartCoroutine(gameObject.FadeOpacity(opacity, animationDuration));
    }

    public void AnimateButton(bool isSelected)
    {
        animator.SetBool("isSelected", isSelected);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();

        valueText = transform.Find("Text/Value").GetComponent<TextMeshProUGUI>();

        smallIcon = transform.Find("Small Icon/Icon").GetComponent<Image>();
        bigIcon = transform.Find("Big Icon/Icon").GetComponent<Image>();
    }

    #endregion
}
