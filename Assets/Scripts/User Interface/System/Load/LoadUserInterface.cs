using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class LoadUserInterface : SystemUserInterfaceBase
{
    #region Constants

    public override int MaxObjects => saveSlots.Count;

    #endregion

    #region Variables

    List<SaveSlot> saveSlots;

    #endregion

    #region Miscellaneous Methods

    public override void SetActive(bool isActive)
    {
        StartCoroutine(LoadUserInterfaceController.Instance.SetActive(isActive));
    }

    public override void UpdateSelectedObject(int selectedValue, int increment = -1)
    {
        StartCoroutine(UpdateSelector(saveSlots[selectedValue].transform));
        UpdateScrollbar(MaxObjects, selectedValue);
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>().ToList();

        selector = transform.Find("Base/Viewport/Selector").gameObject;
        scrollbar = transform.Find("Scrollbar").GetComponent<Scrollbar>();

        base.Awake();
    }

    #endregion
}

