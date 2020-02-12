using System.Collections.Generic;
using UnityEngine;

public class Categorizable : ScriptableObject
{
    #region Fields

    [SerializeField] private int id;
    [SerializeField] private new string name;

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

    #endregion

    #region Variables

    public string description;
    public Category category;

    #endregion

    #region Enums

    public enum Category
    {
 
    }

    #endregion
}
