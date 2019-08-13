using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogSentences //TODO: Change class name, cause it's really ugly
{
    public bool hasChoices;

    [TextArea(1, 3)] //What does this mean?
    public string[] choices;

    [TextArea(1, 10)] //What does this mean?
    public string sentence;
}
