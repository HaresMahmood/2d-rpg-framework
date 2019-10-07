using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New inventory", menuName = "Dialog/Dialog")]
public class Dialog : ScriptableObject
{
    [System.Serializable]
    public class DialogData
    {
        public Character character;
        public string sentence;
        public BranchingDialog branchingDialog;
    }

    public List<DialogData> dialogData = new List<DialogData>();
    public List<DialogData> dialogDataDutch = new List<DialogData>();
    public List<DialogData> dialogDataGerman = new List<DialogData>();
}