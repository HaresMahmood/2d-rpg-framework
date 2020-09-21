using UnityEngine;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class HealthIconAnimation : MonoBehaviour, UIAnimation
{
    #region Variables

    [Header("Settings")]
    [SerializeField, Range(0.01f, 0.5f)] private float animationDuration;

    #endregion

    #region Miscellaneous Methods

    public void Play()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(GetComponent<RectTransform>().DOScale(new Vector3(1.15f, 1.15f, 1f), animationDuration))
        .Append(GetComponent<RectTransform>().DOScale(new Vector3(1f, 1f, 1f), animationDuration))
        .AppendInterval(animationDuration)
        .Append(GetComponent<RectTransform>().DOScale(new Vector3(1.15f, 1.15f, 1f), animationDuration))
        .Append(GetComponent<RectTransform>().DOScale(new Vector3(1f, 1f, 1f), animationDuration));
    }

    #endregion
}

