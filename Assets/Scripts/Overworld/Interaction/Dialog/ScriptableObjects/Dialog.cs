using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/Dialog")]
public class Dialog : ScriptableObject
{
    #region Fields

    [SerializeField] private List<List<DialogData>> data = new List<List<DialogData>>();

    #endregion

    #region Properties

    public List<List<DialogData>> Data
    {
        get { return data; }
    }

    #endregion

    #region Nested Class

    [System.Serializable]
    public class DialogData
    {
        #region Fields

        [SerializeField] private Character character;
        [SerializeField] private string text;
        [SerializeField] private BranchingDialog branch;

        #endregion

        #region Properties

        public Character Character
        {
            get { return character; }
            set { character = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public BranchingDialog Branch
        {
            get { return branch; }
            set { branch = value; }
        }

        #endregion
    }

    #endregion
}