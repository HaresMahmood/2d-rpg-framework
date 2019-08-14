using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogConfig
{
    public bool hasChoices;
    
    public string[] choices;
    
    [TextArea()]
    public string sentence;
}
