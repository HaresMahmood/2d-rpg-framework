using UnityEngine;
using TMPro;
using DG.Tweening;
using CharTween;

/// <summary>
///
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextCircle : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI text;

    private Sequence sequence;

    #endregion

    #region Miscellaneous Methods

    public void AnimateText(string text, int power = 10)
    {
        this.text.text = text;

        
        var tweener = this.text.GetCharTweener();

        /*
        sequence = DOTween.Sequence();

        for (int i = 0; i < text.Length; i++)
        {
            float timeOffset = Mathf.Lerp(0, 1, (i - 0) / (float)(text.Length - 0 + 1));
            var charSequence = DOTween.Sequence();

            //charSequence.Append(tweener.DOCircle(i, 0.1f, 0.5f).SetLoops(-1, LoopType.Restart));
            charSequence.Append(tweener.DOShakePosition(i, 1, 0.05f, 50, 90, false, false));
            sequence.Insert(timeOffset, charSequence);
        }

        sequence.OnComplete(() =>
        {
            //OnFadeComplete?.Invoke(this, EventArgs.Empty);
            Debug.Log(true);
        });

        //sequence.SetLoops(-1, LoopType.Yoyo);
        */

        sequence = DOTween.Sequence();

        //charSequence.Append(tweener.DOCircle(i, 0.1f, 0.5f).SetLoops(-1, LoopType.Restart));
        sequence.Append(GetComponent<CanvasGroup>().DOFade(1f, 0.1f));
        //sequence.Append(transform.DOShakePosition(1f));
        sequence.Append(transform.DOPunchPosition(new Vector3(15, 5, 0), 0.5f, 10 + (100 * (power / 100))));

        sequence.OnComplete(() =>
        {
            sequence.Append(GetComponent<CanvasGroup>().DOFade(0f, 0.15f));
        });

    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    #endregion
}

