using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Branch", menuName = "Dialog/Branch")]
public class BranchingDialog : ScriptableObject
{
    #region Fields

    [SerializeField] private List<DialogBranch> branches = new List<DialogBranch>();

    #endregion

    #region Properties

    public List<DialogBranch> Branches
    {
        get { return branches; }
    }

    #endregion

    #region Nested Classes

    [System.Serializable]
    public class DialogBranch
    {
        #region Fields

        [SerializeField] private string text;
        [SerializeField] private Dialog nextDialog;
        [SerializeField] private UnityEvent branchEvent;

        #endregion

        #region Properties

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Dialog NextDialog
        {
            get { return nextDialog; }
            set { nextDialog = value; }
        }

        public UnityEvent BranchEvent
        {
            get { return branchEvent; }
            set { branchEvent = value; }
        }

        #endregion
    }

    #endregion
}

