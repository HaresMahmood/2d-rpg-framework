#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// </summary>
public class RenameFieldAttribute : PropertyAttribute
{
    public string name { get; private set; }
    public RenameFieldAttribute(string name)
    {
        this.name = name;
    }
}
 
 [CustomPropertyDrawer(typeof(RenameFieldAttribute))]
public class RenameEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, new GUIContent((attribute as RenameFieldAttribute).name));
    }
}

#endif