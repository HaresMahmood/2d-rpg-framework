using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialog/Character")]
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
        male,
        female,
        mixed
    }
}
