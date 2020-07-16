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
    #region Fields

    private static BottomPanelUserInterface instance;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static BottomPanelUserInterface Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BottomPanelUserInterface>();
            }

            return instance;
        }
    }

    #endregion

    #region Variables

    [Header("Settings")]
    [Range(0.01f, 0.2f)] [SerializeField] private float animationTime;
    [Range(0.01f, 0.1f)] [SerializeField] private float delay;

    private List<Transform> buttons;
    private Animator animator;

    #endregion

    #region Miscellaneous Methods

    public IEnumerator ChangePanelButtons(List<PanelButton> panelButtons)
    {
        foreach (Transform button in buttons)
        {
            StartCoroutine(button.gameObject.FadeOpacity(0f, animationTime));
            yield return new WaitForSecondsRealtime(delay);

            if (buttons.IndexOf(button) < panelButtons.Count) // Array.IndexOf(buttons, button) < panelButtons.Count
            {
                // TODO: Debug

                button.GetComponent<LayoutElement>().ignoreLayout = false;
                button.GetComponentInChildren<Image>().sprite = panelButtons[buttons.IndexOf(button)].sprite;
                button.GetComponentInChildren<TextMeshProUGUI>().SetText(panelButtons[buttons.IndexOf(button)].text);
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
            yield return new WaitForSecondsRealtime(delay);
        }
    }

    public IEnumerator AnimateValue(string text, float waitTime)
    {
        //StopAllCoroutines();
        animator.GetComponent<TextMeshProUGUI>().SetText(text);
        animator.SetBool("isToggling", true);
        animator.gameObject.SetActive(true);

        float animationTime = animator.GetAnimationTime();
        waitTime = waitTime < animationTime ? animationTime : waitTime;

        yield return new WaitForSecondsRealtime(waitTime);

        animator.SetBool("isToggling", false); yield return null;
        animationTime = animator.GetAnimationTime();
        yield return new WaitForSecondsRealtime(animationTime);
        animator.gameObject.SetActive(false);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        buttons = transform.Find("Buttons").GetChildren().ToList();
        animator = buttons[(buttons.Count - 1)].GetComponentInChildren<Animator>();
        animator.gameObject.SetActive(false);
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        // Sets bottom panel buttons to first screen user will see.
        
    }

    #endregion
}
