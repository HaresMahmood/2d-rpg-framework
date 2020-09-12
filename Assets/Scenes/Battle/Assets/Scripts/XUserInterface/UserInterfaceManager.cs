using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
///
/// </summary>
public class UserInterfaceManager : MonoBehaviour
{
    #region Fields

    private static UserInterfaceManager instance;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static UserInterfaceManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UserInterfaceManager>();
            }

            return instance;
        }
    }

    #endregion

    #region Variables

    [SerializeField] private ButtonPromptController prompts;

    [Header("Values")]
    [SerializeField] private List<GameObject> stack = new List<GameObject>();

    #endregion

    #region Miscellaneous Methods

    public void PushUserInterface(GameObject newUI)
    {
        if (stack.Count > 0)
        {
            SetActive(stack.Count - 1, false);
        }

        SetActive(newUI, true);
        stack.Add(newUI);
    }

    public void PopUserInterface()
    {
        if (stack.Count > 0)
        {
            SetActive(stack.Count - 1, false);
            stack.RemoveAt(stack.Count - 1);
        }

        SetActive(stack.Count - 1, true);
    }

    private void SetActive(int index, bool isActive)
    {
        stack[index].SetActive(isActive);

        if (isActive)
        {
            prompts.SetInformation(stack[index].GetComponent<ButtonList>().PromptGroups);
        }
    }

    private void SetActive(GameObject ui, bool isActive)
    {
        ui.SetActive(isActive);

        if (isActive)
        {
            prompts.SetInformation(ui.GetComponent<ButtonList>().PromptGroups);
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {

    }

    private void Start()
    {
        
    }

    #endregion
}

