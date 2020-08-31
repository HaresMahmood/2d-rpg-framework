using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class OverworldItemsUserInterfaceController : UserInterfaceController
{
    #region Fields

    private static OverworldItemsUserInterfaceController instance;

    [SerializeField] private OverworldItemsUserInterface userInterface;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static OverworldItemsUserInterfaceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<OverworldItemsUserInterfaceController>();
            }

            return instance;
        }
    }

    public override UserInterface UserInterface
    {
        get { return userInterface; }
    }

    public Item Item { private get; set; }

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        if (isActive)
        {
            selectedValue = ExtensionMethods.IncrementInt(selectedValue, 0, UserInterface.MaxObjects, 1);

            StartCoroutine(userInterface.SetActive(selectedValue, Item));

            yield break;
        }
    }

    #endregion
}

