using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Moves/Move")]
public class Move : ScriptableObject
{
    #region Fields

    [SerializeField] private new string name;
    [SerializeField] private int id;
    [SerializeField] private string description;
    [SerializeField] private int pp;
    [SerializeField] private int accuracy;
    [SerializeField] private int power;
    [SerializeField] private MoveCategory category;
    [SerializeField] private Typing typing;

    #endregion

    #region Properties

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public int PP
    {
        get { return pp; }
        set { pp = value; }
    }

    public int Accuracy
    {
        get { return accuracy; }
        set { accuracy = value; }
    }

    public int Power
    {
        get { return power; }
        set { power = value; }
    }

    public MoveCategory Category
    {
        get { return category; }
        set { category = value; }
    }

    public Typing Typing
    {
        get { return typing; }
    }

    #endregion

    #region Enums

    public enum MoveCategory
    {
        Special,
        Physical
    }

    #endregion
}