using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
///
/// </summary>
public class ButtonList : MonoBehaviour
{
    /*
    #region Constats

    private const int SIZE = 

    #endregion
    */

    #region Fields

    [SerializeField] private List<ButtonPrompt> promptGroups = new List<ButtonPrompt>();

    #endregion

    #region Properties

    public List<ButtonPrompt> PromptGroups
    {
        get { return promptGroups; }
    }

    #endregion

    #region Nested Classes

    [System.Serializable]
    public class ButtonPrompt
    {
        #region Fields

        [SerializeField] private string text;
        [SerializeField] private InputActionReference action;
        [SerializeField] private int value; // TODO: Make conditional
        [Space(5)]
        [SerializeField] private UnityEvent onClick;
        [SerializeField] private List<Prompt> prompts = new List<Prompt>();

        #endregion

        #region Properties

        public string Text
        {
            get { return text; }
        }

        public InputActionReference Action
        {
            get { return action; }
        }
        
        public int Value
        {
            get { return value; }
        }

        public UnityEvent OnClick
        {
            get { return onClick; }
        }

        public List<Prompt> Prompts
        {
            get { return prompts; }
        }

        #endregion

        #region Nested Class

        [System.Serializable]
        public class Prompt
        {
            #region Fields

            [SerializeField] private string text;
            [SerializeField] private Sprite icon;

            #endregion

            #region Properties

            public string Text
            {
                get { return text; }
            }

            public Sprite Icon
            {
                get { return icon; }
            }

            #endregion
        }

        #endregion
    }

    #endregion
}

