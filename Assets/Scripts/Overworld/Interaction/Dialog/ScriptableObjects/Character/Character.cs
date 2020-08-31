using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "Characters/NPC/Generic")]
public class Character : ScriptableObject
{
    #region Fields

    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] private CharacterGender gender;

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

    public CharacterGender Gender
    {
        get { return gender; }
        set { gender = value; }
    }

    #endregion

    #region Enums

    public enum CharacterGender
    {
        Male,
        Female,
        Mixed
    }

    #endregion
}
