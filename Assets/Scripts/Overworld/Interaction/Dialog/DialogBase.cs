using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/Dialog")]
public class DialogBase : ScriptableObject
{
    [System.Serializable]
    public class Info
    {
        public CharacterInfo character;
        [TextArea(1, 4)]
        public string sentence;
        public DialogChoices choices;
    }

    [Header("Dialog information")]
    public Info[] dialogInfo;
}
