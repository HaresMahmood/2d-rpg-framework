using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class BattleUIAnimation : MonoBehaviour // TODO: Derive from "UIAnimation" class.
{

    // TODO: Add sub-ui to party ui to test animation!!!


    #region Variables

    [Header("Setup")]
    [SerializeField] private RectTransform content; // *

    [Header("Settings")]
    [SerializeField, Range(0.01f, 2f)] private float animationTime = 0.3f;

    private RectTransform background; // *

    Sequence sequence;

    #endregion

    #region Miscellaneous Methods

    public void Animate(bool isActive)
    {
        sequence = DOTween.Sequence();

        if (isActive)
        {
            gameObject.SetActive(isActive);
            LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());

            background.localScale = new Vector3(1f, 1f, 1f);
            background.anchoredPosition = new Vector2(background.GetComponent<RectTransform>().anchoredPosition.x, -75f);
            background.sizeDelta = new Vector2(background.GetComponent<RectTransform>().sizeDelta.x, 0f);

            content.localScale = new Vector3(1f, 1f, 1f);
            content.anchoredPosition = new Vector2(background.GetComponent<RectTransform>().anchoredPosition.x, -250f);
            content.sizeDelta = new Vector2(background.GetComponent<RectTransform>().sizeDelta.x, 0f);

            sequence.Append(background.GetComponent<CanvasGroup>().DOFade(1f, animationTime));
            sequence.Join(background.DOAnchorPosY(16.75f, animationTime));
            sequence.Join(background.DOSizeDelta(new Vector2(background.GetComponent<RectTransform>().sizeDelta.x, 33.5f), animationTime));
            sequence.Join(content.GetComponent<CanvasGroup>().DOFade(1f, animationTime));
            sequence.Join(content.DOAnchorPosY(0f, animationTime));
            sequence.Join(content.DOSizeDelta(new Vector2(background.GetComponent<RectTransform>().sizeDelta.x, 0f), animationTime));
        }
        else
        {
            sequence.Append(background.GetComponent<CanvasGroup>().DOFade(0f, animationTime));
            sequence.Join(content.GetComponent<CanvasGroup>().DOFade(0f, animationTime));
            sequence.Join(content.DOScale(new Vector2(0.9f, 0.95f), animationTime));

            sequence.OnComplete(() =>
            {
                gameObject.SetActive(isActive);
            });
        }
    }

    #endregion
    
    #region Unity Methods
    
    private void Awake()
    {
        background = transform.Find("Background").GetComponent<RectTransform>();
    }

    #endregion
}

