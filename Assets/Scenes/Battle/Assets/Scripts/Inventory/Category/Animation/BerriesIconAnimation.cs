using UnityEngine;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class BerriesIconAnimation : MonoBehaviour, UIAnimation
{
    #region Variables

    [Header("Settings")]
    [SerializeField, Range(0.01f, 0.5f)] private float animationDuration;

    #endregion

    #region Miscellaneous Methods

    public void Play()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.Find("Icon/Main").GetComponent<RectTransform>().DORotate(new Vector3(0f, 0f, 17.5f), animationDuration))
        //.Join(transform.Find("Icon/Dependent").GetComponent<RectTransform>().DOAnchorPosY())
        .Append(transform.Find("Icon/Main").GetComponent<RectTransform>().DORotate(new Vector3(0f, 0f, 0f), animationDuration))
        .Append(transform.Find("Icon/Main").GetComponent<RectTransform>().DORotate(new Vector3(0f, 0f, -17.5f), animationDuration))
        .Append(transform.Find("Icon/Main").GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 0), animationDuration));
    }

    #endregion
}

