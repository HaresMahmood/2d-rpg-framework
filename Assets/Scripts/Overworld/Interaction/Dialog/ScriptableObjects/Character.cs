using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "Characters/NPC")]
public class Character : ScriptableObject
{
    public new string name;
    public int id;
    public Sprite portrait;
    public Gender gender;
    public bool battleable;
    //TODO: Create fields for trainers/battleable NPCs.

    public enum Gender
    {
        Male,
        Female,
        Mixed
    }
}
