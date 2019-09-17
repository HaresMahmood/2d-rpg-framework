[System.Serializable]
public class DialogInfo1
{
    public Character character;
    public string sentence;
    public BranchingDialog choices;

    public DialogInfo1(Character _character, string _sentence, BranchingDialog _choices)
    {
        character = _character;
        sentence = _sentence;
        choices = _choices;
    }
}
