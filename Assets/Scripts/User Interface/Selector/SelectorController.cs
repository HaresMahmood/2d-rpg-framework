using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class SelectorController : MonoBehaviour // TODO: Make animation times serialzable!
{
    #region Variables

    private Sequence sequence;

    #endregion

    #region Miscellaneous Methods

    public void UpdatePosition(Transform selectedObject)
    {
        Sequence sequence = DOTween.Sequence();

        this.sequence.Kill();

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        sequence.Append(GetComponent<CanvasGroup>().DOFade(0f, 0.15f));

        sequence.OnComplete(() =>
        {
            sequence.Kill();

            if (selectedObject != null)
            {
                transform.position = selectedObject.position;
                Animate();
            }
            else
            {
                gameObject.SetActive(false);
            }
        });
    }

    public void Deactivate()
    {
        Sequence sequence = DOTween.Sequence();

        this.sequence.Kill();

        sequence.Append(GetComponent<RectTransform>().DOScale(0.99f, 0.05f))
            .Append(GetComponent<CanvasGroup>().DOFade(0f, 0.15f))
            .Join(GetComponent<RectTransform>().DOScale(1.025f, 0.07f))
            .Append(GetComponent<RectTransform>().DOScale(1f, 0.08f));

        sequence.OnComplete(() =>
        {
            sequence.Kill();
            gameObject.SetActive(false);
        });
    }

    private void Animate()
    {
        this.sequence = DOTween.Sequence();

        GetComponent<CanvasGroup>().alpha = 0.5f;

        this.sequence.Append(GetComponent<CanvasGroup>().DOFade(1f, 0.5f))
            .Append(GetComponent<CanvasGroup>().DOFade(0.5f, 0.5f))
            .Append(GetComponent<CanvasGroup>().DOFade(1f, 0.5f));

        this.sequence.SetLoops(-1, LoopType.Yoyo);
    }

    /*
        Sequence sequence = DOTween.Sequence();
        this.sequence = DOTween.Sequence();

        sequence.Append(GetComponent<CanvasGroup>().DOFade(0.5f, 0.15f));

        sequence.OnComplete(() =>
        {
            this.sequence.Append(GetComponent<CanvasGroup>().DOFade(1f, 0.5f))
                .Append(GetComponent<CanvasGroup>().DOFade(0.5f, 0.5f));
        });

        this.sequence.SetLoops(-1, LoopType.Restart);
    */

    #endregion

    #region Unity Methods


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        Animate();
    }

    #endregion
}

