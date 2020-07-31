using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class OverworldItemsUserInterface : UserInterface
{
    #region Constants

    public override int MaxObjects => 0;

    #endregion

    #region Variables

    List<OverworldItemPanel> panels;

    #endregion

    #region Miscellaneous Methods

    public IEnumerator SetActive(int selectedValue)
    {
        panels[selectedValue].gameObject.SetActive(true);

        yield return null;  yield return new WaitForSecondsRealtime(panels[selectedValue].GetComponent<Animator>().GetAnimationTime());

        panels[selectedValue].gameObject.SetActive(false);
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
    }

    #endregion
}

