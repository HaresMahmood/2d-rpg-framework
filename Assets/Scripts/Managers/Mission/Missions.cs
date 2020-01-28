using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Missions", menuName = "Missions/Missions")]
public class Missions : ScriptableObject
{
    public List<Mission> mission = new List<Mission>();
}