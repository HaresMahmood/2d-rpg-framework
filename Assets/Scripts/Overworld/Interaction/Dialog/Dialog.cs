using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/Dialog")]
public class Dialog : ScriptableObject
{
    [System.Serializable]
    public class Info
    {
        public CharacterInfo character;
        [TextArea] public string sentence;
        public DialogChoices choices;
    }
    
    public Info[] dialogInfo;
}
