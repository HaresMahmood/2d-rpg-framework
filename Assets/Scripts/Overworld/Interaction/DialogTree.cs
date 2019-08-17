using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogTree
{
    public bool hasChoices;
    
    public string[] choices;

    public int jumpToSentence;
    
    [TextArea()]
    public string sentence;
}
