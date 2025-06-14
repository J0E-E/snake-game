using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private InputField playerInitialsField;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        ManagerLocator.Register<GameManager>(this);

        RegisterManagers();
    }

    private void RegisterManagers()
    {
        var managers = GetComponents<MonoBehaviour>();

        foreach (var manager in managers)
        {
            if (manager is IManager)
            {
                var type = manager.GetType();
                var registerMethod = typeof(ManagerLocator).GetMethod("Register").MakeGenericMethod(type);
                registerMethod.Invoke(null, new object[] { manager });
            }
        }
    }

    public void ChangeScenes(SceneName scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public void StartGame()
    {
        ChangeScenes(SceneName.Snake);
    }

    public int GetCurrentLevel()
    {
        return 1;
    }
}
