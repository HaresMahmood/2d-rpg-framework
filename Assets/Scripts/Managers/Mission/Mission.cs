using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Missions/Mission")]
public class Mission : ScriptableObject
{
    #region Fields

    [SerializeField] private int id;

    #endregion

    #region Properties

    public string Name;
    public int ID
    {
        get { return id; }
        private set { id = value; }
    }

    #endregion

    #region Variables

    public string objective;
    public string description;
    public Category category;
    public string remaining;
    public Character assignee;
    public string location;
    public string rewardAmount;
    public Item rewardItem;
    public bool isCompleted;

    #endregion

    #region Enums

    public enum Category
    {
        Main,
        Side,
        Placeholder
    }

    #endregion
}
