using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///
/// </summary>
public class BottomPanelUserInterface : MonoBehaviour
{
    #region Variables

     [Range(0.01f, 0.2f)] [SerializeField] private float animationTime;

    #endregion

    #region Miscellaneous Methods

    public IEnumerator ChangePanelButtons(List<PanelButton> buttons)
    {
        foreach (Transform button in transform.Find("Buttons").GetChildren())
        {
            StartCoroutine(button.gameObject.FadeOpacity(0f, animationTime));
            yield return new WaitForSecondsRealtime(animationTime);

            if (Array.IndexOf(transform.Find("Buttons").GetChildren(), button) < buttons.Count)
            {
                button.GetComponent<LayoutElement>().ignoreLayout = false;
                button.GetComponentInChildren<Image>().sprite = buttons[Array.IndexOf(transform.Find("Buttons").GetChildren(), button)].sprite;
                button.GetComponentInChildren<TextMeshProUGUI>().SetText(buttons[Array.IndexOf(transform.Find("Buttons").GetChildren(), button)].text);
            }
            else
            {
                button.GetComponent<LayoutElement>().ignoreLayout = true;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.Find("Buttons").GetComponent<RectTransform>());
        }

        yield return new WaitForSecondsRealtime(animationTime * 1.5f);

        for (int i = 0; i < buttons.Count; i++)
        {
            StartCoroutine(transform.Find("Buttons").GetChildren()[i].gameObject.FadeOpacity(1f, animationTime));
            yield return new WaitForSecondsRealtime(animationTime);
        }
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
