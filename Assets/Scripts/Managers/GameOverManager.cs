using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private GameManager GameManager => ManagerLocator.Get<GameManager>();
    public void RestartGame()
    {
        GameManager.ChangeScenes(SceneName.Snake);
    }
}
