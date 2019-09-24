using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New dialog branch", menuName = "Dialog/Dialog branch")]
public class BranchingDialog : ScriptableObject
{
    [System.Serializable]
    public class DialogBranch
    {
        public string branchOption;
        public Dialog nextDialog;
        public UnityEvent branchEvent;
    }

    public List<DialogBranch> dialogBranches = new List<DialogBranch>();
}

