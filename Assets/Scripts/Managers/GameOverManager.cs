using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private GameManager GameManager => ManagerLocator.Get<GameManager>();
    public void RestartGame()
    {
        GameManager.ChangeScenes(SceneName.Snake);
    }

    public void QuitGame()
    {
        // Close the game
        Application.Quit();

        // For editor testing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
