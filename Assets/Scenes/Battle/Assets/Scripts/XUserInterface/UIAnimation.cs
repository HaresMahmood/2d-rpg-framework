using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class UIAnimation : MonoBehaviour 
{

    // TODO: Add sub-ui to party ui to test animation!!!


    // TODO: Chang ename to reflect it's only used in BattleSystem UI


    // TODO: Set all canvasgroups to rectransform!!*



    #region Variables

    [Header("Setup")]
    [SerializeField] private CanvasGroup content; // *

    [Header("Settings")]
    [SerializeField, Range(0.01f, 2f)] private float animationTime = 0.3f;

    private CanvasGroup background; // *

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

            background.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            background.GetComponent<RectTransform>().anchoredPosition = new Vector2(background.GetComponent<RectTransform>().anchoredPosition.x, -75f);
            background.GetComponent<RectTransform>().sizeDelta = new Vector2(background.GetComponent<RectTransform>().sizeDelta.x, 0f);

            content.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            content.GetComponent<RectTransform>().anchoredPosition = new Vector2(background.GetComponent<RectTransform>().anchoredPosition.x, -250f);
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(background.GetComponent<RectTransform>().sizeDelta.x, 0f);

            sequence.Append(background.DOFade(1f, animationTime));
            sequence.Join(background.GetComponent<RectTransform>().DOAnchorPosY(16.75f, animationTime));
            sequence.Join(background.GetComponent<RectTransform>().DOSizeDelta(new Vector2(background.GetComponent<RectTransform>().sizeDelta.x, 33.5f), animationTime));
            sequence.Join(content.DOFade(1f, animationTime));
            sequence.Join(content.GetComponent<RectTransform>().DOAnchorPosY(0f, animationTime));
            sequence.Join(content.GetComponent<RectTransform>().DOSizeDelta(new Vector2(background.GetComponent<RectTransform>().sizeDelta.x, 0f), animationTime));

            sequence.OnComplete(() =>
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
            });
        }
        else
        {
            sequence.Append(background.DOFade(0f, animationTime));
            sequence.Join(content.DOFade(0f, animationTime));
            sequence.Join(content.GetComponent<RectTransform>().DOScale(new Vector2(0.9f, 0.95f), animationTime));

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
        background = transform.Find("Background").GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    #endregion
}

