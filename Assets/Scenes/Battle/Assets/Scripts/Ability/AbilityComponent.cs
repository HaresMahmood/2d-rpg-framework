using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class AbilityComponent : MonoBehaviour
{
    #region Variables

    [Header("Values")] [SerializeField]
    private List<ComponentInformation> list; // TODO: Change to Queue (Was not working as expected)

    #endregion 

    #region Miscellaneous Methods

    public void SetInformation(Ability ability, bool reverseArrangement)
    {
        list.Add(new ComponentInformation(ability, reverseArrangement));

        Animate();
    }

    private void SetInformation(ComponentInformation information)
    {

        GetComponentInChildren<TextMeshProUGUI>().SetText(information.Ability.Name);
        GetComponent<HorizontalLayoutGroup>().reverseArrangement = information.ReverseArrangement;

        transform.Find("Arrow").eulerAngles = new Vector3(
        transform.eulerAngles.x,
        transform.eulerAngles.y,
        Convert.ToInt32(GetComponent<HorizontalLayoutGroup>().reverseArrangement) * -180);
    }

    private void Animate()
    {
        Sequence sequence = DOTween.Sequence();

        SetInformation(list[0]);

        sequence.Append(GetComponent<CanvasGroup>().DOFade(1f, 0.1f));
        //sequence.Join(transform.DOMoveY());
        sequence.AppendInterval(2f);
        sequence.Append(GetComponent<CanvasGroup>().DOFade(0f, 0.15f));

        sequence.OnComplete(() =>
        {
            if (list.Count > 1)
            {
                list.RemoveAt(0);
                Animate();
            }
        });
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        list = new List<ComponentInformation>();
    }

    #endregion

    #region Nested Classes   

    [Serializable]
    internal class ComponentInformation // TODO: Kinda bad name
    {
        internal Ability Ability { get; set; }

        internal bool ReverseArrangement { get; set; }

        internal ComponentInformation(Ability ability, bool revereArrangement)
        {
            Ability = ability;
            ReverseArrangement = revereArrangement;
        }
    }

    #endregion
}

