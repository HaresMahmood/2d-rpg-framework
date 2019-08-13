using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Dialog
{
    [UnityEngine.Header("General")]
    //[UnityEngine.Header("Properties)]   TODO: Use to group bools?

    public bool hasName;
    public string name;

    public DialogSentences[] sentences; //TODO: Change name of DialogSentences.
}
