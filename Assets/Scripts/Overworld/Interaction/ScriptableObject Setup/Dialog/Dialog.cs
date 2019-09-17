using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    [System.Serializable]
    public class DialogInfo
    {
        public Character character;
        public string sentence;
        public BranchingDialog choices;
    }

    public List<DialogInfo> dialog = new List<DialogInfo>();
}
