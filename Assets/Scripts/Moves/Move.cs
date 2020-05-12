using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Moves/Move")]
public class Move : ScriptableObject
{
    public new string name;
    public int id;
    public int pp;
    public Category category;
    public int accuracy;
    public int power;
    public string description;
    public Typing typing;

    public enum Category
    {
        None,
        Special,
        Physical
    }
}