using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string name;
    public bool hasOptions;

    [TextArea(1, 10)]
    public string[] sentences;

    [TextArea(1, 3)]
    public string[] options;
}
