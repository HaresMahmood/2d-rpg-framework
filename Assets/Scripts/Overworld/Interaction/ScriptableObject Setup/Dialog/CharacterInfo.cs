using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialog/Character")]
public class CharacterInfo : ScriptableObject
{
    public new string name;
    public Sprite portrait;
}
