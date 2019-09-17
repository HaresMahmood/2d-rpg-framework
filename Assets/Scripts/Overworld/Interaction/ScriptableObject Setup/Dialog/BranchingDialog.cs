using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialog Branch", menuName = "Dialog/Dialog Branch")]
public class BranchingDialog : ScriptableObject
{
    [System.Serializable]
    public class Choices
    {
        public string choiceText;
        public Dialog nextDialog;
        public UnityEvent choiceEvent;

    }

    public List<Choices> branchingDialog = new List<Choices>();
}
