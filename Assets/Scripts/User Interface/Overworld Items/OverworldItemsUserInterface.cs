using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class OverworldItemsUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => panels.Count;

    #endregion

    #region Variables

    private List<OverworldItemPanel> panels;

    private List<OverworldItemPanel> activePanels;

    #endregion

    #region Miscellaneous Methods

    public IEnumerator SetActive(int selectedValue, Item item)
    {
        panels[selectedValue].SetInformation(item);
        panels[selectedValue].gameObject.SetActive(true);
        StartCoroutine(ActivatePanel(selectedValue));

        while (activePanels.Count != 0)
        {
            yield return null;
        }

        panels[selectedValue].gameObject.SetActive(false);
    }

    private IEnumerator ActivatePanel(int selectedValue)
    {
        activePanels.Add(panels[selectedValue]);

        yield return null; yield return new WaitForSecondsRealtime(panels[selectedValue].GetComponent<Animator>().GetAnimationTime());

        activePanels.Remove(panels[selectedValue]);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        panels = GetComponentsInChildren<OverworldItemPanel>().ToList();
        panels.Reverse();

        foreach (OverworldItemPanel panel in panels)
        {
            panel.gameObject.SetActive(false);
        }

        activePanels = new List<OverworldItemPanel>();
    }

    #endregion
}

