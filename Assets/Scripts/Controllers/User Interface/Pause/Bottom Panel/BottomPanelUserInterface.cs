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
    [Range(0.01f, 0.2f)] [SerializeField] private float animationDuration;
    [Range(0.01f, 0.1f)] [SerializeField] private float delay;

    private List<MenuContextButton> buttons;

    #endregion

    #region Miscellaneous Methods

    public IEnumerator ChangePanelButtons(List<PanelButton> menuButtons)
    {
        /*
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
        */



        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].FadeButton(0f, buttons[i].gameObject.activeSelf ? animationDuration : -1);

            yield return new WaitForSecondsRealtime(delay);
        }

        yield return new WaitForSecondsRealtime(animationDuration); //  + (delay * buttons.Count)

        for (int i = 0; i < menuButtons.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].SetValues(menuButtons[i].text, menuButtons[i].icon, menuButtons[i].isAnimated);
        }

        if (menuButtons.Count < buttons.Count)
        {
            for (int i = menuButtons.Count; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < menuButtons.Count; i++)
        {
            buttons[i].FadeButton(1f, animationDuration);

            yield return new WaitForSecondsRealtime(delay);
        }

        //LayoutRebuilder.ForceRebuildLayoutImmediate(transform.Find("Buttons").GetComponent<RectTransform>());
    }

    public void AnimateButton(int index, string value)
    {
        buttons[index].AnimateButton(value);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        buttons = GetComponentsInChildren<MenuContextButton>().ToList();
    }

    #endregion
}
