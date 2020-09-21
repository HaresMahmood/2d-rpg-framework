using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class PokeBallsIconAnimation : MonoBehaviour, UIAnimation
{
    #region Variables

    [Header("Settings")]
    [SerializeField, Range(0.01f, 0.5f)] private float animationDuration;

    #endregion

    #region Miscellaneous Methods

    public void Play()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 25), animationDuration))
        .Append(GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 0), animationDuration))
        .AppendInterval(animationDuration)
        .Append(GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -25), animationDuration))
        .Append(GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 0), animationDuration));
    }

    #endregion
}

