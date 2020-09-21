using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

    #region Events

    [Header("Events"), Space(5)]
    [SerializeField] private UnityEvent OnStart;

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
        if (stack[index].GetComponent<BattleUIAnimation>() != null)
        {
            stack[index].GetComponent<BattleUIAnimation>().Animate(isActive);
        }
        else
        {
            stack[index].SetActive(isActive);
        }

        if (isActive)
        {
            EventSystem.current.SetSelectedGameObject(stack[index].GetComponent<ButtonList>().FirstSelected);
            prompts.SetInformation(stack[index].GetComponent<ButtonList>().PromptGroups);
        }
    }

    private void SetActive(GameObject ui, bool isActive)
    {
        if (ui.GetComponent<BattleUIAnimation>() != null)
        {
            ui.GetComponent<BattleUIAnimation>().Animate(isActive);
        }
        else
        {
            ui.SetActive(isActive);
        }

        if (isActive)
        {
            EventSystem.current.SetSelectedGameObject(ui.GetComponent<ButtonList>().FirstSelected);
            prompts.SetInformation(ui.GetComponent<ButtonList>().PromptGroups);
        }
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        OnStart.Invoke();
    }

    #endregion
}

