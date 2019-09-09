using UnityEngine;

public class StartScene : MonoBehaviour 
{
	/// <summary>
    /// The name of the scene to load at Start.
	/// </summary>
	[Tooltip("The scene loaded at the start of the game.")]
	public string startSceneName = "SampleScene";

	public void Start() 
	{
		SceneStreamManager.SetCurrentScene(startSceneName);
		Destroy(this);
	}
}
