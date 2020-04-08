using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    #region Fields

    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] private string description;
    [SerializeField] private bool requiresTarget;
    [SerializeField] private bool cancastOnSelf;
    //private GameObject particleEffect;
    //private (Unity)Event trigger;

    #endregion

    #region Properties

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    #endregion
}
