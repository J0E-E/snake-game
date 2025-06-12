using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private readonly GameManager _gameManager = ManagerLocator.Get<GameManager>();
    public void StartGame()
    {
        _gameManager.ChangeScenes(SceneName.Snake);
    }
}
