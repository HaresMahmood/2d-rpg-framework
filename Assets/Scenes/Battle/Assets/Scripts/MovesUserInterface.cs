using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class MovesUserInterface : MonoBehaviour
{
    #region Variables

    private List<Button> buttons;

    #endregion

    #region Miscellaneous Methods

    public void DeselectButtons(Button selectedButton)
    {
        List<Button> buttons = this.buttons.Where(b => b != selectedButton).ToList();

        foreach (Button button in buttons)
        {
            //if (button.)

            button.transform.Find("Text").gameObject.SetActive(false);
            button.transform.Find("Selector").gameObject.SetActive(false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<RectTransform>());
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>().ToList();

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<MoveButton>().Index = i;
        }
    }

    private void Start()
    {
        for (int i = 1; i < buttons.Count; i++)
        {
            buttons[i].transform.Find("Text").gameObject.SetActive(false);
            buttons[i].transform.Find("Selector").gameObject.SetActive(false);
        }
    }

    #endregion
}

