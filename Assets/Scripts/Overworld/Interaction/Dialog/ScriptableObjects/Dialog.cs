using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/Dialog")]
public class Dialog : ScriptableObject
{
    #region Fields

    [SerializeField] private List<DialogLanguageData> data = new List<DialogLanguageData>();

    #endregion

    #region Properties

    public List<DialogLanguageData> Data
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

        public bool ShowBranch { get; set; }

        #endregion
    }

    [System.Serializable]
    public class DialogLanguageData
    {
        #region Fields

        [SerializeField] private List<DialogData> languageData = new List<DialogData>();
        [SerializeField] private string language;

        #endregion

        #region Properties

        public List<DialogData> LanguageData
        {
            get { return languageData; }
        }

        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        #endregion
    }

    #endregion
}