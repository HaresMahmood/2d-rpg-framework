using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialog/Character")]
public class Character : ScriptableObject
{
    public new string name;
    public Sprite portrait;
}
