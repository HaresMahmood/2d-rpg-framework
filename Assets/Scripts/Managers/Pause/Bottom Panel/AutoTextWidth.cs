using UnityEngine;
using TMPro;

/// <summary>
///
/// </summary>
public class AutoTextWidth : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI textComponent;

    #endregion

    #region Miscellaneous Methods

    public void UpdateWidth(string text)
    {
        Vector2 textSize = new Vector2(textComponent.GetPreferredValues(text).x, textComponent.rectTransform.sizeDelta.y);
        textComponent.rectTransform.sizeDelta = textSize;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        UpdateWidth(textComponent.text);
    }

    #endregion
}
