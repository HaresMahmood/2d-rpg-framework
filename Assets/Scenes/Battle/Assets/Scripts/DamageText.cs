using UnityEngine;
using TMPro;
using DG.Tweening;
using CharTween;

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

    #region Miscellaneous Methods

    public void AnimateText(string text, int power = 10)
    {
        this.text.GetComponent<TextMeshProUGUI>().text = text;
        this.textShadow.GetComponent<TextMeshProUGUI>().text = text;

        sequence = DOTween.Sequence();

        //charSequence.Append(tweener.DOCircle(i, 0.1f, 0.5f).SetLoops(-1, LoopType.Restart));
        sequence.Append(GetComponent<CanvasGroup>().DOFade(1f, 0.1f));
        sequence.Append(textShadow.DOPunchPosition(new Vector3(15, 5, 0), 0.5f, 10 + (100 * (power / 100))));
        sequence.Join(this.text.DOPunchPosition(new Vector3(15, 5, 0), 0.5f, 20 + (100 * (power / 100))));

        sequence.OnComplete(() =>
        {
            sequence.Append(GetComponent<CanvasGroup>().DOFade(0f, 0.15f));
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

