using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class OverworldItemsUserInterfaceController : UserInterfaceController
{
    [InspectorButton("OnButtonClicked")]
    public bool isActive;

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

    #endregion

    #region Miscellaneous Methods

    public override IEnumerator SetActive(bool isActive, bool condition = true)
    {
        if (isActive)
        {
            Debug.Log(true);

            StartCoroutine(userInterface.SetActive(++selectedValue));

            yield break;
        }
    }

    private void OnButtonClicked()
    {
        StartCoroutine(SetActive(true));
    }

    #endregion
}

