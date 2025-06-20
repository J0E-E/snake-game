using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private GameManager GameManager => ManagerLocator.Get<GameManager>();
    private PlayerManager PlayerManager => ManagerLocator.Get<PlayerManager>();

    [SerializeField] private InputField initialsInputField;
    [SerializeField] private Button startGameButton;

    private void Start()
    {
        initialsInputField.characterLimit = 4;
    }

    public void SetPlayerInitials(string initials)
    {
        initialsInputField.text = initials.ToUpper();
        startGameButton.interactable = initials.Length > 0;
        PlayerManager.SetPlayerInitials(initials.ToUpper());
    }

    public void StartGame()
    {
        GameManager.StartGame();
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