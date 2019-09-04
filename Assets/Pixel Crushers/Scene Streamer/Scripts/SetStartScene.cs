using UnityEngine;
using System.Collections;

[AddComponentMenu("Scene Streamer/Set Start Scene")]
	public class SetStartScene : MonoBehaviour 
	{

		/// <summary>
		/// The name of the scene to load at Start.
		/// </summary>
		[Tooltip("Load this scene at start")]
		public string startSceneName = "Scene 1";

		public void Start() 
		{
			SceneStreamer.SetCurrentScene(startSceneName);
			Destroy(this);
		}

	}
