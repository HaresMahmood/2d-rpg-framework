using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New branch", menuName = "Dialog/Branch")]
public class BranchingDialog : ScriptableObject
{
    [System.Serializable]
    public class DialogBranch
    {
        public string branchOption;
        public Dialog nextDialog;
        public UnityEvent branchEvent;
        public bool hasBackButton;
    }

    public List<DialogBranch> dialogBranches = new List<DialogBranch>();
}

