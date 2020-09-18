using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class InventoryDetailsComponent : UserInterfaceComponent
{
    #region Variables

    private CanvasGroup information;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI effectText;
    private TextMeshProUGUI descriptionText;

    #endregion

    #region Miscellaneous Methods

    public void Animate(Item item, float animationDuration)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(information.DOFade(0f, animationDuration));

        if (item != null)
        {
            sequence.OnComplete(() =>
            {
                sequence = DOTween.Sequence();

                SetInformation(item);

                sequence.Append(information.DOFade(1f, animationDuration));
            });
        }
    }

    private void SetInformation(Item information)
    {
        nameText.SetText(information.Name);
        descriptionText.SetText(information.Description);
    }

    public override void SetInspectorValues()
    {
        information = transform.Find("Information").GetComponent<CanvasGroup>();

        nameText = information.transform.Find("Basic Information/Name").GetComponent<TextMeshProUGUI>();
        effectText = information.transform.Find("Basic Information/Effect").GetComponent<TextMeshProUGUI>();
        descriptionText = information.transform.Find("Description/Value").GetComponent<TextMeshProUGUI>();
    }

    #endregion

    #region Unity Methods

    protected override void Start()
    { }

    #endregion
}

