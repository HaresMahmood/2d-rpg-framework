using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

/// <summary>
///
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class DamageText : MonoBehaviour
{
    #region Variables

    private Transform text;
    private Transform textShadow;

    private Sequence sequence;

    #endregion

    #region Events

    public event EventHandler OnAnimationComplete;

    #endregion

    #region Miscellaneous Methods

    public void AnimateText(int value)
    {
        text.GetComponent<TextMeshProUGUI>().text = value.ToString();
        textShadow.GetComponent<TextMeshProUGUI>().text = value.ToString();

        sequence = DOTween.Sequence();

        //charSequence.Append(tweener.DOCircle(i, 0.1f, 0.5f).SetLoops(-1, LoopType.Restart));
        sequence.Append(GetComponent<CanvasGroup>().DOFade(1f, 0.1f));
        sequence.Append(textShadow.DOPunchPosition(new Vector3(15, 5, 0), 0.5f, 10 + (100 * (value / 100))));
        sequence.Join(this.text.DOPunchPosition(new Vector3(15, 5, 0), 0.5f, 20 + (100 * (value / 100))));
        sequence.Append(GetComponent<CanvasGroup>().DOFade(0f, 0.15f));

        sequence.OnComplete(() =>
        {
            OnAnimationComplete?.Invoke(this, EventArgs.Empty);
        });
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        textShadow = transform.Find("Value (Shadow)");
        text = textShadow.Find("Value");
    }

    #endregion
}

