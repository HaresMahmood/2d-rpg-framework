using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Categorizable/Mission")]
public class Mission : Categorizable
{
    #region Fields

    [SerializeField] private MissionCategory categorization = new MissionCategory();
    [SerializeField] private MissionOriginDestination originDestination = new MissionOriginDestination();
    [SerializeField] private List<MissionGoal> goals = new List<MissionGoal>();
    [SerializeField] private List<MissionReward> rewards = new List<MissionReward>();

    #endregion

    #region Properties

    public override Category Categorization
    {
        get { return categorization; }
    }

    public MissionOriginDestination OriginDestination
    {
        get { return originDestination; }
    }

    public List<MissionGoal> Goals
    {
        get { return goals; }
    }

    public List<MissionReward> Rewards
    {
        get { return rewards; }
    }

    public int CompletionPercentage
    {
        get
        {
            return (int)Math.Round(Goals.Where(g => g.IsCompleted == true).Count() / (float)Goals.Count() * 100);
        }
    }

    public bool IsFailed
    {
        get 
        {
            return Goals.Where(g => g.IsFailed == true).Count() > 0;
        }
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

    [Serializable]
    public class MissionOriginDestination
    {
        #region Fields

        [SerializeField] private Character assignee;
        [SerializeField] private string origin;
        [SerializeField] private string destination;

        #endregion

        #region Properties

        public Character Assignee
        {
            get { return assignee; }
            set { assignee = value; }
        }

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        #endregion
    }

    [System.Serializable]
    public class MissionGoal
    {
        #region Fields

        [SerializeField] private GoalType type;
        [SerializeField] private bool isCompleted;
        [SerializeField] private bool isFailed;

        [SerializeField] private Character character;
        [SerializeField] private Pokemon pokemon;
        [SerializeField] private Item item;

        [SerializeField] private int amount;
         
        #endregion

        #region Properties

        public GoalType Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

        public bool IsFailed
        {
            get { return isFailed; }
            set { isFailed = value; }
        }

        public Character Character
        {
            get { return character; }
            set { character = value; }
        }

        public Pokemon Pokemon
        {
            get { return pokemon; }
            set { pokemon = value; }
        }

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        #endregion

        #region Enums

        public enum GoalType
        {
            Talk,
            Defeat,
            Gather,
            Deliver,
            Escort
        }

        #endregion
    }

    [System.Serializable]
    public class MissionReward
    {
        #region Fields

        [SerializeField] private RewardType type;
        [SerializeField] private int amount;
        [SerializeField] private Item item;

        #endregion

        #region Properties

        public RewardType Type
        {
            get { return type; }
            set { type = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }

        #endregion

        #region Enums

        public enum RewardType
        {
            Money,
            Item,
            Experience
        }

        #endregion
    }

    #endregion
}
