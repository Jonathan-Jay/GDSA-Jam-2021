using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
	public bool startGame = false;
	public int loadedScene = 0;

	#region SingletonCode
	private static SceneManager _instance;
	public static SceneManager Instance { get { return _instance; } }
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}

        //loads start menu
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
		loadedScene = 1;
		startGame = true;
	}
	#endregion

	//Use this function to swap scenes
	public void SwapScene(int scene)
	{
		UnloadScene(loadedScene);
		LoadScene(scene);
	}

	private void LoadScene(int scene)
	{
		loadedScene = scene;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
	}

	private void UnloadScene(int scene)
	{
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
	}

}
