using UnityEngine;

public class SceneController : MonoBehaviour
{
    public void OnStartPressed()
    {
        SceneManager.Instance.SwapScene(2);
    }
    public void OnQuitPressed()
    {
        Application.Quit();
    }
    public void OnReturnPressed()
    {
        SceneManager.Instance.SwapScene(1);
    }
    // TODO Haider: trigger end game here
    public void OnGameEnded()
    {
        SceneManager.Instance.SwapScene(3);
    }

}
