using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Categorizable/Mission")]
public class Mission : Categorizable
{
    #region Fields

    [SerializeField] private MissionCategory categorization = new MissionCategory();
    [SerializeField] private string objective;
    [SerializeField] private string remaining;
    [SerializeField] private Character assignee;
    [SerializeField] private string origin;
    [SerializeField] private string destination;
    [SerializeField] private List<MissionReward> rewards = new List<MissionReward>();
    [SerializeField] private bool isCompleted;
    [SerializeField] private bool isFailed;

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

    public string Origin
    {
        get { return origin; }
        private set { origin = value; }
    }

    public string Destination
    {
        get { return destination; }
        private set { destination = value; }
    }

    public List<MissionReward> Rewards
    {
        get { return rewards; }
    }

    public bool IsCompleted
    {
        get { return isCompleted; }
        private set { isCompleted = value; }
    }

    public bool IsFailed
    {
        get { return isFailed; }
        private set { isFailed = value; }
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

        public Category EnumValue
        {
            get { return value; }
            set { this.value = value; }
        }

        protected override string Value
        {
            get { return value.ToString(); }
        }

        #endregion

        #region Enums

        public enum Category
        {
            Main,
            Side
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

        [SerializeField] private Type type;

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

        #region Enums

        private enum Type
        {
            Money,
            Item,
            Experience
        }

        #endregion
    }

    #endregion
}
