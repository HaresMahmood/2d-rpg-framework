using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Categorizable/Mission")]
public class Mission : Categorizable
{
    #region Fields

    [SerializeField] private MissionCategory categorization = new MissionCategory();
    [SerializeField] private string objective;
    [SerializeField] private string remaining;
    [SerializeField] private Character assignee;
    [SerializeField] private string location;
    [SerializeField] private MissionReward reward;
    [SerializeField] private bool isCompleted;

    #endregion

    #region Properties

    public override Category Categorization
    {
        get { return categorization; }
    }

    public string Objective
    {
        get { return objective; }
        private set { objective = value; }
    }

    public string Remaining
    {
        get { return remaining; }
        private set { remaining = value; }
    }

    public Character Assignee
    {
        get { return assignee; }
        private set { assignee = value; }
    }

    public string Location
    {
        get { return location; }
        private set { location = value; }
    }

    public MissionReward Reward
    {
        get { return reward; }
        private set { reward = value; }
    }

    public bool IsCompleted
    {
        get { return isCompleted; }
        private set { isCompleted = value; }
    }

    #endregion

    #region Nested Classes

    [System.Serializable]
    public sealed class MissionCategory : Category
    {
        #region Variables

        [SerializeField] private Category value;

        #endregion

        #region Properties

        protected override string Value
        {
            get { return value.ToString(); }
        }

        #endregion

        #region Enums

        private enum Category
        {
            Main,
            Side,
            Other
        }

        #endregion

        #region Miscellaneous Methods

        public override string GetCategoryFromIndex(int index)
        {
            return ((Category)index).ToString();
        }

        public override int GetTotalCategories()
        {
            return Enum.GetNames(typeof(Category)).Length;
        }

        #endregion
    }

    [System.Serializable]
    public class MissionReward
    {
        #region Fields

        [SerializeField] private int amount;
        [SerializeField] private Item item;

        #endregion

        #region Properties

        public int Amount
        {
            get { return amount; }
            private set { amount = value; }
        }

        public Item Item
        {
            get { return item; }
            private set { item = value; }
        }

        #endregion
    }

    #endregion
}
