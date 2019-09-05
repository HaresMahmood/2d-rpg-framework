﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameObjectEventsHandler : MonoBehaviour
{
    //Dictionary of specific scene gameobjects
    public static Dictionary<string, List<GameObject>> specificSceneObjects = new Dictionary<string, List<GameObject>>();

    private void Update()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
    }

    //Listen to the events
    public void OnEnable()
    {
        GameObjectEvents.notifyAwake += HandlenotifyAwake;
        GameObjectEvents.notifyDeath += HandlenotifyDeath;
    }

    public void OnDisable()
    {
        GameObjectEvents.notifyAwake -= HandlenotifyAwake;
        GameObjectEvents.notifyDeath -= HandlenotifyDeath;
    }

    public void HandlenotifyAwake(GameObject obj)
    {
        //if there are no objects for this scene then create a new list with the current scene list name
        if (!specificSceneObjects.ContainsKey(obj.scene.name))
            specificSceneObjects.Add(obj.scene.name, new List<GameObject>());

        //now add the gameobject which sent this event upon awake/start to this list
        specificSceneObjects[obj.scene.name].Add(obj);
    }

    public void HandlenotifyDeath(GameObject obj)
    {
        
        //if the dicitonary has this object then remove it upon object destroy
        if (specificSceneObjects.ContainsKey(SceneManager.GetActiveScene().name))
        {
            if (specificSceneObjects[obj.scene.name].Contains(obj))
                specificSceneObjects[obj.scene.name].Remove(obj);
        }
    }
}