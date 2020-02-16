using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Categorizable")]
public abstract class Categorizable : ScriptableObject
{
    #region Fields

    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] private string description;

    #endregion

    #region Properties

    public string Name
    {
        get { return name; }
        private set { name = value; }
    }

    public int ID
    {
        get { return id; }
        private set { id = value; }
    }

    public string Description
    {
        get { return description; }
        private set { description = value; }
    }

    public Category Categorization { get; }

    #endregion

    #region Nested Classes

    [System.Serializable]
    public abstract class Category
    {
        #region Properies

        public virtual string Value { get; }

        #endregion

        #region Miscellaneous Methods

        public override string ToString()
        {
            return Value;
        }

        public virtual string GetCategoryFromIndex(int index)
        { 
            return null;  
        }

        public virtual int GetTotalCategories()
        {
            return 0;
        }

        #endregion


    }

    #endregion
}
