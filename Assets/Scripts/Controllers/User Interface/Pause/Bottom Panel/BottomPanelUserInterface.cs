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

    [Header("Settings")]
    [Range(0.01f, 0.2f)] [SerializeField] private float animationTime;

    private Transform[] buttons;
    private Animator valueAnimator;

    #endregion

    #region Miscellaneous Methods

    public IEnumerator ChangePanelButtons(List<PanelButton> panelButtons)
    {
        foreach (Transform button in buttons)
        {
            StartCoroutine(button.gameObject.FadeOpacity(0f, animationTime));
            yield return new WaitForSecondsRealtime(animationTime / 2);

            if (Array.IndexOf(buttons, button) < panelButtons.Count)
            {
                button.GetComponent<LayoutElement>().ignoreLayout = false;
                button.GetComponentInChildren<Image>().sprite = panelButtons[Array.IndexOf(buttons, button)].sprite;
                button.GetComponentInChildren<TextMeshProUGUI>().SetText(panelButtons[Array.IndexOf(buttons, button)].text);
                button.GetComponentInChildren<AutoTextWidth>().UpdateWidth(button.GetComponentInChildren<TextMeshProUGUI>().text);
            }
            else
            {
                button.GetComponent<LayoutElement>().ignoreLayout = true;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.Find("Buttons").GetComponent<RectTransform>());
        }

        yield return new WaitForSecondsRealtime(animationTime * 1.5f);

        for (int i = 0; i < panelButtons.Count; i++)
        {
            StartCoroutine(buttons[i].gameObject.FadeOpacity(1f, animationTime));
            yield return new WaitForSecondsRealtime(animationTime / 2);
        }
    }

    public IEnumerator AnimateValue(string text, float waitTime)
    {
        //StopAllCoroutines();
        valueAnimator.GetComponent<TextMeshProUGUI>().SetText(text);
        valueAnimator.SetBool("isToggling", true);
        valueAnimator.gameObject.SetActive(true);

        float animationTime = valueAnimator.GetAnimationTime();
        waitTime = waitTime < animationTime ? animationTime : waitTime;

        yield return new WaitForSecondsRealtime(waitTime);

        valueAnimator.SetBool("isToggling", false); yield return null;
        animationTime = valueAnimator.GetAnimationTime();
        yield return new WaitForSecondsRealtime(animationTime);
        valueAnimator.gameObject.SetActive(false);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        buttons = transform.Find("Buttons").GetChildren();
        valueAnimator = buttons[(buttons.Length - 1)].GetComponentInChildren<Animator>();
        valueAnimator.gameObject.SetActive(false);
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        // Sets bottom panel buttons to first screen user will see.
        
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        
    }

    #endregion
}
