using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class AbilityComponent : MonoBehaviour
{
    #region Miscellaneous Methods

    public void SetInformation(Ability ability, bool reverseArrangement)
    {
        GetComponentInChildren<TextMeshProUGUI>().SetText(ability.Name);
        GetComponent<HorizontalLayoutGroup>().reverseArrangement = reverseArrangement;

        Animate();
        SetOrientation();
    }

    private void Animate()
    {
         Sequence sequence = DOTween.Sequence();

        //charSequence.Append(tweener.DOCircle(i, 0.1f, 0.5f).SetLoops(-1, LoopType.Restart));
        sequence.Append(GetComponent<CanvasGroup>().DOFade(1f, 0.1f));
        //sequence.Join(transform.DOMoveY());
        sequence.AppendInterval(2f);
        sequence.Append(GetComponent<CanvasGroup>().DOFade(0f, 0.15f));

        sequence.OnComplete(() =>
        {
            //gameObject.SetActive(false);
        });
    }

    private void SetOrientation()
    {
        //GetComponentInChildren<TextMeshProUGUI>().alignment = GetComponent<HorizontalLayoutGroup>().reverseArrangement ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;

        transform.Find("Arrow").eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            Convert.ToInt32(GetComponent<HorizontalLayoutGroup>().reverseArrangement) * -180);
    }

    #endregion

    #region Unity Methods

    #endregion
}

